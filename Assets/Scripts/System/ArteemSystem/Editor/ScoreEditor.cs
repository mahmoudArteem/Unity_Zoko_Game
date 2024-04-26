using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ScoreDataObject))]
public class ScoreEditor : Editor
{
    ScoreDataObject sdo;
    int listCount = 0;
    bool[] folds;
    public override void OnInspectorGUI()
    {
        sdo = (ScoreDataObject)target;
        EditorGUILayout.BeginVertical();
        GUI.color = Color.blue;
        if (GUILayout.Button("Refresh"))
        {
            sdo.refreshData();
            folds = new bool[listCount];
        }

        GUI.color = Color.green;
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(sdo);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        EditorGUILayout.EndVertical();
        GUI.color = Color.white;

        generateList();
        //base.OnInspectorGUI();

    }

    void generateList()
    {
        listCount = sdo._scoreData.Count;

        for(int i = 0; i < folds.Length; i++)
        {
            folds[i] = EditorGUILayout.Foldout(folds[i], "Level " + i.ToString(), true);
            EditorGUILayout.BeginVertical();
            if (folds[i])
            {
                sdo._scoreData[i].starAValue = EditorGUILayout.IntField("Star A", sdo._scoreData[i].starAValue);
                sdo._scoreData[i].starBValue = EditorGUILayout.IntField("Star B", sdo._scoreData[i].starBValue);
                sdo._scoreData[i].starCValue = EditorGUILayout.IntField("Star C", sdo._scoreData[i].starCValue);
            }
            EditorGUILayout.EndVertical();

        }

    }

}
