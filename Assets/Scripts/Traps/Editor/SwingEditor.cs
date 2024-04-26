using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TrapSwing))]
public class SwingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TrapSwing ts = (TrapSwing)target;

        if (GUILayout.Button("Refresh")) {

            ts.generatChain();

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        base.OnInspectorGUI();
    }
}
