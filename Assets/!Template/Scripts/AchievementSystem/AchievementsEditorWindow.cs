#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class AchievementsEditorWindow : EditorWindow
{
    private AchievimentsData targetData;
    private SerializedObject serializedData;
    private SerializedProperty achievementsProp;

    private Type[] behaviourTypes = new Type[0];
    private string[] behaviourTypeNames = new string[0];

    [MenuItem("SGames/Achievements Editor")]
    public static void OpenWindow() => GetWindow<AchievementsEditorWindow>("Achievements Editor");

    private void OnEnable()
    {
        RefreshBehaviourTypes();
    }

    private void RefreshBehaviourTypes()
    {
        // собираем все непустые, не-абстрактные наследники AchievementBehaviour
        behaviourTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(a => {
                try { return a.GetTypes(); }
                catch { return new Type[0]; }
            })
            .Where(t => t != null && typeof(AchievementBehaviour).IsAssignableFrom(t) && !t.IsAbstract)
            .ToArray();

        behaviourTypeNames = behaviourTypes.Select(t => t.Name).ToArray();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        targetData = (AchievimentsData)EditorGUILayout.ObjectField("Achieviments Data", targetData, typeof(AchievimentsData), false);
        if (EditorGUI.EndChangeCheck())
        {
            serializedData = null; // пересоздадим SerializedObject при смене
        }

        if (targetData == null)
        {
            EditorGUILayout.HelpBox("Выберите AchievimentsData (ScriptableObject) для редактирования.", MessageType.Info);
            if (GUILayout.Button("Загрузить из Resources/AchievimentsData"))
            {
                targetData = AchievimentsData.AchivFromResource;
                serializedData = null;
            }
            return;
        }

        if (serializedData == null || serializedData.targetObject != targetData)
        {
            serializedData = new SerializedObject(targetData);
            achievementsProp = serializedData.FindProperty("allAchiviements");
        }

        serializedData.Update();

        if (achievementsProp == null)
        {
            EditorGUILayout.HelpBox("Поле 'allAchiviements' не найдено в выбранном AchievimentsData. Убедитесь, что в классе оно называется именно так и помечено [SerializeField].", MessageType.Error);
            return;
        }

        EditorGUILayout.LabelField("Achievements", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        for (int i = 0; i < achievementsProp.arraySize; i++)
        {
            var element = achievementsProp.GetArrayElementAtIndex(i);
            var infoProp = element.FindPropertyRelative("Info");
            var behaviourProp = element.FindPropertyRelative("achievementBehaviour");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(infoProp, new GUIContent("Info"), true);

            // Построим popup опций: [<None>, Type1, Type2, ...]
            string[] popupOptions = new string[behaviourTypeNames.Length + 1];
            popupOptions[0] = "<None>";
            for (int k = 0; k < behaviourTypeNames.Length; k++) popupOptions[k + 1] = behaviourTypeNames[k];

            int currentIndex = -1;
            var currentObj = behaviourProp.managedReferenceValue;
            if (currentObj != null)
            {
                var t = currentObj.GetType();
                currentIndex = Array.IndexOf(behaviourTypes, t);
            }

            int popupIndex = currentIndex + 1;
            int newPopupIndex = EditorGUILayout.Popup("Behaviour", popupIndex, popupOptions);

            if (newPopupIndex != popupIndex)
            {
                if (newPopupIndex == 0)
                {
                    behaviourProp.managedReferenceValue = null;
                }
                else
                {
                    Type chosen = behaviourTypes[newPopupIndex - 1];
                    behaviourProp.managedReferenceValue = Activator.CreateInstance(chosen);
                }
            }

            // Рисуем поля конкретного behaviour (если не null)
            if (behaviourProp.managedReferenceValue != null)
            {
                // Toggle foldout? Для простоты — прямо PropertyField (работает с [SerializeReference])
                EditorGUILayout.PropertyField(behaviourProp, new GUIContent("Behaviour"), true);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove Achievement"))
            {
                achievementsProp.DeleteArrayElementAtIndex(i);
                // если DeleteArrayElementAtIndex оставляет "разорванную" ссылку (Unity), вызываем второй раз:
                if (achievementsProp.GetArrayElementAtIndex(i) != null && achievementsProp.GetArrayElementAtIndex(i).isArray)
                {
                    // noop
                }
                break; // выходим, потому что структура массива изменилась
            }
            if (GUILayout.Button("Clear Behaviour"))
            {
                behaviourProp.managedReferenceValue = null;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Achievement"))
        {
            // Добавляем новый элемент и инициализируем
            int insertIndex = achievementsProp.arraySize;
            achievementsProp.arraySize++;
            serializedData.ApplyModifiedProperties(); // чтобы Unity создала элемент
            serializedData.Update();

            var newElem = achievementsProp.GetArrayElementAtIndex(insertIndex);
            var newInfo = newElem.FindPropertyRelative("Info");
            if (newInfo != null)
            {
                var idProp = newInfo.FindPropertyRelative("ID");
                if (idProp != null) idProp.stringValue = "NewAchievement";
            }
            var newBehaviour = newElem.FindPropertyRelative("achievementBehaviour");
            if (newBehaviour != null)
            {
                newBehaviour.managedReferenceValue = null;
            }
        }

        if (GUILayout.Button("Refresh Behaviour Types"))
        {
            RefreshBehaviourTypes();
        }
        EditorGUILayout.EndHorizontal();

        serializedData.ApplyModifiedProperties();

        if (GUI.changed)
            EditorUtility.SetDirty(targetData);
    }
}
#endif