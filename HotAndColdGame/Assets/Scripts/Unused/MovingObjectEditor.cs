using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingObject))]
public class MovingObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MovingObject obj = (MovingObject)target;

        if (GUILayout.Button("Set start Pos"))
            obj.SetStartPosition();
        if (GUILayout.Button("Set End Pos"))
            obj.SetEndPosition();

    }
}
