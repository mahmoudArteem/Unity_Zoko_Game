using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TrapBox))]
public class TrapBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TrapBox tb = (TrapBox)target;

        if (GUILayout.Button("Refresh"))
        {
            tb.generateTrap();


            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        base.OnInspectorGUI();
    }
}
