using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GameMaster))]
public class GameMasterInspector : Editor
{

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameMaster gm = (GameMaster)target;

        if (GUILayout.Button("Save Current Difficulty to PlayerPrefs"))
        {
            gm.SetDifficulty(gm.difficultyNum);
        }
        
        if (GUILayout.Button("Load Difficulty from PlayerPrefs"))
        {
            gm.LoadDifficulty();
        }   
    }

}
