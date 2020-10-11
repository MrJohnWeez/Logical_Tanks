using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyScriptEditor))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Find and Kill UndoProRecords"))
        {
            Selection.activeGameObject = GameObject.Find("UndoProRecords");
            DestroyImmediate(Selection.activeGameObject);
        }
    }
}
