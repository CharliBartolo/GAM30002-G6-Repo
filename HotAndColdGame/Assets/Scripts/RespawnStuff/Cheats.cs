using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for letting Andy skip through the level
/// 
/// By: Jason - 02/10/2021
/// </summary>

public class Cheats : MonoBehaviour
{
    [SerializeField] public CheckPoint spawn;               //reference checkpoint script
    //[SerializeField] public GameObject checkPointList;      //gameobject in scene with all checkpoints as the children
    [SerializeField] public List<Transform> checkPoints;    //list for saving checkpoint positions
    public int num = 0;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnLevelLoad;
        checkPoints = new List<Transform>();
        searchForCheckpoints();
        //creates a list of checkpoints in current scene
        //for (int i = 0; i < checkPointList.transform.childCount; ++i)
        //{
            //access the positions of the grandchildren
        //    checkPoints.Add(checkPointList.transform.GetChild(i).transform);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(checkPoints[num].name);
        //next checkpoint
        if ((Input.GetKeyDown("1")) && (num < checkPoints.Count))
        {
            num++;
            //Debug.Log("Adding");
            CheckpointTeleport();
        }
        //previous checkpoint
        else if ((Input.GetKeyDown("2")) && (num > 0)) 
        {
            num--;
            //Debug.Log("Minusing");
            CheckpointTeleport();
        }
    }

    //purpose of function is for Andy test different partd of the level
    void CheckpointTeleport()
    {
        Debug.Log("loading scene");
        //make last checkpoint equal selected checkpoint
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameMaster.instance.playerRef.transform.position = checkPoints[num].position;
        GameMaster.instance.playerRef.transform.rotation = checkPoints[num].rotation;
        spawn.spawnPos = checkPoints[num];
        //loads scene at last saved checkpoint
        
        //GameMaster.instance.LoadCheckpoint();
        //GameMaster.instance.playerRef.transform.position = checkPoints[num].position;
    }

    public void searchForCheckpoints() 
    {
        checkPoints.Clear();
        num = 0;
        //GameObject.FindGameObjectsWithTag("Campfire");        
        GameObject[] ObjectsFound = GameObject.FindGameObjectsWithTag("Respawn");
        //Debug.Log("Enter loop");
        foreach (GameObject objects in ObjectsFound) 
        {
            //Debug.Log(objects.name);

            if (objects.GetComponent<CheckPoint>() != null) 
            {
                checkPoints.Add(objects.GetComponent<CheckPoint>().spawnPos);
                //Debug.Log("checkpoint added");
            }
        }
    }

    public void OnLevelLoad(Scene load, LoadSceneMode mode)
    {
        searchForCheckpoints();
    }
}
