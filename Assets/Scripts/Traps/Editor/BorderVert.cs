using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor (typeof(LaserBorderVer))]
public class BorderVert : Editor
{
    public override void OnInspectorGUI()
    {
        LaserBorderVer lvb = (LaserBorderVer)target;
        base.OnInspectorGUI();

        GUILayout.BeginVertical();

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Refresh"))
        {
            lvb.generateTrapBorder();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Flip"))
        {
            lvb.flip();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        GUILayout.EndVertical();
    }
}
