using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for respawning player at last checkpoint
/// 
/// By: Jason - 09/10/2021
/// </summary>

public class Reload : MonoBehaviour
{
    [SerializeField] public CheckPoint spawn;               //reference checkpoint script
    [SerializeField] public List<Transform> checkPoints;    //list for saving checkpoint positions
    private GameMaster gm;                                  //reference game master script

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnLevelLoad;
        checkPoints = new List<Transform>();
        searchForCheckpoints();
        //get game master last position
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //Debug.Log("GM Found");
    }

    //for button press 
    public void OnButtonPress() 
    {
        //Debug.Log("Reloading");
        CheckpointTeleport();
    }

    //purpose of function is respawn player at last checkpoint
    void CheckpointTeleport()
    {
        //goes through list of checkpoints in scene
        for (int i = 0; i < checkPoints.Count; ++i) 
        {
            //checks if list has position of last checkpoint position
            if (checkPoints[i] = gm.lastCheckPointPos) 
            {
                GameMaster.instance.playerRef.transform.position = checkPoints[i].position;
                GameMaster.instance.playerRef.transform.rotation = checkPoints[i].rotation;
                spawn.spawnPos = checkPoints[i];
                //Debug.Log(checkPoints[i]);
            }
        }
    }

    //find checkpoints in scene
    public void searchForCheckpoints()
    {
        checkPoints.Clear();      
        GameObject[] ObjectsFound = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject objects in ObjectsFound)
        {
            if (objects.GetComponent<CheckPoint>() != null)
            {
                checkPoints.Add(objects.GetComponent<CheckPoint>().spawnPos);
            }
        }
    }

    //when scene load find checkpoints in scene
    public void OnLevelLoad(Scene load, LoadSceneMode mode)
    {
        searchForCheckpoints();
    }
}
