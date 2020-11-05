using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveTester))]
public class SaveTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveTester tester = (SaveTester)target;

        if (GUILayout.Button("Create Palette"))
        {
            tester.CreatePalette();
        }

        if (GUILayout.Button("Add objects to palette"))
        {
            tester.AddObjectListToPalette();
        }
    }
}
