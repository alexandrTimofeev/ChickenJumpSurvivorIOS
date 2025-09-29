#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class GameSessionDataEditor : EditorWindow
{
    private GameSessionDataContainer runtimeInstance;
    private GameSessionDataContainer resourceAsset;

    private Vector2 scroll;

    [MenuItem("SGames/Session Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameSessionDataEditor>("Game Session Data");
    }

    private void OnEnable()
    {
        LoadData();
    }

    private void OnHierarchyChange()
    {
        LoadData();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Game Session Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (Application.isPlaying && runtimeInstance != null)
        {
            EditorGUILayout.HelpBox("Редактируется активная копия GameSessionDataContainer (Runtime)", MessageType.Info);
            DrawData(runtimeInstance);
        }
        else
        {
            EditorGUILayout.HelpBox("Редактируется оригинальный GameSessionDataContainer (Resources)", MessageType.Warning);
            if (resourceAsset != null)
            {
                DrawData(resourceAsset);
            }
            else
            {
                EditorGUILayout.HelpBox("Файл Resources/GameSessionDataContainer не найден", MessageType.Error);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(this);
        }
    }

    private void LoadData()
    {
        if (Application.isPlaying)
        {
            runtimeInstance = GetContainerFrom();
        }
        else
        {
            resourceAsset = Resources.Load<GameSessionDataContainer>("GameSessionDataContainer");
        }
    }

    private GameSessionDataContainer GetContainerFrom()
    {
        return GameEntryGameplayCCh.DataContainer;
    }

    private void DrawData(GameSessionDataContainer container)
    {
        if (container == null)
        {
            EditorGUILayout.HelpBox("Container не найден", MessageType.Error);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.LabelField("HealthContainer", EditorStyles.boldLabel);
        if (container.HealthContainer != null)
        {
            container.HealthContainer.SetValue(EditorGUILayout.IntField("Value", container.HealthContainer.Value));
            container.HealthContainer.SetClampRange(new Vector2Int(EditorGUILayout.IntField("Min Value", container.HealthContainer.ClampRange.x),
                EditorGUILayout.IntField("Max Value", container.HealthContainer.ClampRange.y)));
        }
        else
        {
            EditorGUILayout.HelpBox("HealthContainer = null", MessageType.Warning);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Game Speed", EditorStyles.boldLabel);
        container.SpeedGame = EditorGUILayout.FloatField("SpeedGame", container.SpeedGame);

        EditorGUILayout.Space(10);
        container.StandartLevelData = EditorGUILayout.TextField("StandartLevelData", container.StandartLevelData);

        EditorGUILayout.EndScrollView();

        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(container);
        }
    }
}
#endif