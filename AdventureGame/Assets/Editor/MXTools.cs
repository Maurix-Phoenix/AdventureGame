using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MXTools : EditorWindow
{
    [MenuItem("MX Tools/MX Tools Window")]
    public static void ShowWindow()
    {
        GetWindow<MXTools>("MX Tools");
    }

    private void OnGUI()
    {
        GUILayout.Label("Test Label", EditorStyles.boldLabel);

        GUILayout.Button("Test Button");
    }
}
