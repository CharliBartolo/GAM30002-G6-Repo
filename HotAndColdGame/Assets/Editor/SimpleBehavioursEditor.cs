using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleBehaviours))]
public class SimpleBehavioursEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimpleBehaviours obj = (SimpleBehaviours)target;


        if (GUILayout.Button("Set Start Position"))
            obj.SetStartPosition();

        if (GUILayout.Button("Set End Position"))
            obj.SetEndPosition();

    }
}
