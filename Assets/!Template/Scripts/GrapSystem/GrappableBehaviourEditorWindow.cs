#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GrappableBehaviourEditorWindow : EditorWindow
{
    private GrappableObjectBehaviour targetBehaviour;
    private Vector2 scrollPosition;

    [MenuItem("SGames/Edit Grappable Behaviour")]
    public static void ShowWindow()
    {
        GetWindow<GrappableBehaviourEditorWindow>("Grappable Behaviour Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grappable Object Behaviour Editor", EditorStyles.boldLabel);

        targetBehaviour = (GrappableObjectBehaviour)EditorGUILayout.ObjectField("Target Behaviour", targetBehaviour, typeof(GrappableObjectBehaviour), false);
        if (targetBehaviour == null) return;

        EditorGUILayout.Space();
        DrawAddActionMenu();
        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        SerializedObject serializedObject = new SerializedObject(targetBehaviour);
        SerializedProperty actionsProperty = serializedObject.FindProperty("behaviourActions");

        for (int i = 0; i < actionsProperty.arraySize; i++)
        {
            SerializedProperty element = actionsProperty.GetArrayElementAtIndex(i);
            SerializedProperty titleProp = element.FindPropertyRelative("Title");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(element, new GUIContent($"#{i} - {element.managedReferenceFullTypename.Split(' ').Last()}"), true);
            if (GUILayout.Button("Remove"))
            {
                actionsProperty.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetBehaviour);
        }
    }

    private void DrawAddActionMenu()
    {
        if (GUILayout.Button("Add New Action"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var type in GetAllDerivedTypes<GrappableObjectBehaviourAction>())
            {
                var localType = type;
                menu.AddItem(new GUIContent(localType.Name), false, () => AddNewAction(localType));
            }
            menu.ShowAsContext();
        }
    }

    private void AddNewAction(Type actionType)
    {
        var action = (GrappableObjectBehaviourAction)Activator.CreateInstance(actionType);
        Undo.RecordObject(targetBehaviour, "Add Grappable Action");
        targetBehaviour.behaviourActions.Add(action);
        EditorUtility.SetDirty(targetBehaviour);
    }

    private static List<Type> GetAllDerivedTypes<T>()
    {
        var baseType = typeof(T);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => GetSafeTypes(asm))
            .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
            .ToList();
    }

    private static IEnumerable<Type> GetSafeTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }
}
#endif