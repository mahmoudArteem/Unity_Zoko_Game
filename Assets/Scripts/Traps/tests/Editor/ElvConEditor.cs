using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ElevatorController))]
public class ElvConEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ElevatorController elv = (ElevatorController)target;
        if (GUILayout.Button("Refresh"))
        {
            elv.generateElevator();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        base.OnInspectorGUI();
    }
}
