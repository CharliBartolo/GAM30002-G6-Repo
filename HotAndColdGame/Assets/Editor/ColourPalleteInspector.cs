using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ColourPalette))]
public class ColourPalleteInspector : Editor
{
    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefaultInspector();

        ColourPalette col = (ColourPalette)target;

        if (GUILayout.Button("Save Colour Preferences to Slot 1"))
        {
            col.SaveColourPrefs(1);
        }
        
        if (GUILayout.Button("Save Colour Preferences to Slot 2"))
        {
            col.SaveColourPrefs(2);
        }

        if (GUILayout.Button("Load Colour Preferences to Slot 1"))
        {
            col.LoadColourPrefs(1);
        }

        if (GUILayout.Button("Load Colour Preferences to Slot 2"))
        {
            col.LoadColourPrefs(2);
        }
    }
}

