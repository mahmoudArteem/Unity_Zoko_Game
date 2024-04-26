using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor (typeof(LaserBorderHorz))]
public class BorderHorz : Editor
{
    public override void OnInspectorGUI()
    {
        LaserBorderHorz lhb = (LaserBorderHorz)target;
        base.OnInspectorGUI();

        GUILayout.BeginVertical();

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Refresh"))
        {
            lhb.generateTrapBorder();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Flip"))
        {
            lhb.flip();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        GUILayout.EndVertical();
    }
}
