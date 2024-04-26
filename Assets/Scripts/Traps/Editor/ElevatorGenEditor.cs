using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ElevatorGenerator))]
public class ElevatorGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ElevatorGenerator eh = (ElevatorGenerator)target;
        base.OnInspectorGUI();

        GUILayout.BeginVertical();

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Generate"))
        {
            eh.generateTrap();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        GUILayout.EndVertical();
    }
}
