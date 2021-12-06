using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveLevelDesignEditorWindow : EditorWindow
{    
    bool newLevel;
    public int removeLevelAt = 0;
    public int replaceLevelAt = 0;


    [MenuItem("Tools/Save Level Design")]
    public static void OpenWindow()
    {
        GetWindow<SaveLevelDesignEditorWindow>("Save Level");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("ADD LEVELS", EditorStyles.boldLabel);
        AddNewLevel();
        GUILayout.Space(10);

        GUI.enabled = true;
        GUILayout.Label("SAVE LEVEL",EditorStyles.boldLabel);
        SaveButton();

    }

    void AddNewLevel()
    {

        if (!newLevel)
        {
            GUI.enabled = true;
        }
        else
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("NEW LEVEL"))
        {
            newLevel = true;
        }
    }
    void SaveButton()
    {
        if (newLevel)
        {
            GUI.enabled = true;
        }
        else
        {
            GUI.enabled = false;
        }

        if (GUILayout.Button("SAVE"))
        {
            LevelManager.Instance.SaveLevelData();
            newLevel = false;
        }
    }
}
