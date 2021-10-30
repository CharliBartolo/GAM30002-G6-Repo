using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// This class is responsible for respawning player at last checkpoint
/// Last edit: UI now automatically closes and selected triggers are reset after retry button is pressed  
/// By: Jason - 29/10/2021
/// </summary>

public class Reload : MonoBehaviour
{
    //[SerializeField] public CheckPoint spawn;               //reference checkpoint script
    [SerializeField] public List<Transform> checkPoints;    //list for saving checkpoint positions
    [SerializeField] public List<DeadArea> Trigger;    //list for saving checkpoint positions
    private GameMaster gm;                                  //reference game master script
    //public static event Action<bool> PowerButtonClicked = delegate { };
    private PlayerController pauseUI;
    //private Button UI;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnLevelLoad;
        checkPoints = new List<Transform>();
        searchForCheckpoints();
        //get game master last position
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //Debug.Log("GM Found");
        pauseUI = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //for button press 
    public void OnButtonPress() 
    {
        //Debug.Log("Reloading");
        CheckpointTeleport();
        for (int i = 0; i < Trigger.Count; ++i)
        {
            Trigger[i].TriggerReset();
            //Debug.Log("Working");
        }
    }

    //purpose of function is respawn player at last checkpoint
    void CheckpointTeleport()
    {
        //goes through list of checkpoints in scene
        for (int i = 0; i < checkPoints.Count; ++i) 
        {
            //checks if list has position of last checkpoint position
            if (checkPoints[i] == gm.lastCheckPointPos) 
            {
                GameMaster.instance.playerRef.transform.position = checkPoints[i].position;
                GameMaster.instance.playerRef.transform.rotation = checkPoints[i].rotation;
                gm.lastCheckPointPos = checkPoints[i];
                //Debug.Log(checkPoints[i]);
                //when line below is run then the UI closes but wasd doesn't move player but cursor works
                pauseUI.PC.IsPaused = false;
                Time.timeScale = pauseUI.PC.IsPaused ? 0 : 1;
                //Debug.Log("UI closed");
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
