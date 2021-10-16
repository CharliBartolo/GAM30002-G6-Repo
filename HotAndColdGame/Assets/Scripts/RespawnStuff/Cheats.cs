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
    public bool isAdditiveLevelLoadEnabled = false;
    public bool isHUDVisible = true;
    public bool isGUNVisible = true;
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
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (num < checkPoints.Count - 1)
                num++;
            else
                Debug.Log("Already hit last checkpoint!");
            //Debug.Log("Adding");
            CheckpointTeleport();
        }
        //previous checkpoint
        else if ((Input.GetKeyDown(KeyCode.Minus)) )
        {
            if (num > 0)
                num--;
            else
                Debug.Log("Already at first checkpoint!");
            CheckpointTeleport();
        }

        if (Input.GetKeyDown("h"))
        {
            GameMaster.instance.playerRef.GetComponent<ReticleFXController>().isHidden =
                !GameMaster.instance.playerRef.GetComponent<ReticleFXController>().isHidden;

            FindObjectOfType<UIFXController>().isTemperatureHidden = !FindObjectOfType<UIFXController>().isTemperatureHidden;

          
        }

        if (Input.GetKeyDown("g"))
        {
            if(isGUNVisible)
            {
                GameMaster.instance.playerRef.GetComponent<GunFXController>().arm_obj.transform.Find("Arm_Armature").localScale = Vector3.zero;
                //GameMaster.instance.playerRef.gameObject.transform.Find("Arm_Armature").localScale = Vector3.zero;
                isGUNVisible = false;
            }
            else
            {
                GameMaster.instance.playerRef.GetComponent<GunFXController>().arm_obj.transform.Find("Arm_Armature").localScale = Vector3.one;
                //GameMaster.instance.playerRef.gameObject.transform.Find("Arm_Armature").localScale = Vector3.one;
                isGUNVisible = true;
            }
        }

        if (Input.GetKeyDown("u"))
        {
            GameMaster.instance.savedGunState += 1;
            GameMaster.instance.LoadGunState();
        }
    }
    //purpose of function is for Andy test different partd of the level
    void CheckpointTeleport()
    {
        if (checkPoints.Count < 1)
            return;
        //Debug.Log("loading scene");
        //make last checkpoint equal selected checkpoint
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameMaster.instance.playerRef.transform.position = checkPoints[num].position;
        GameMaster.instance.playerRef.transform.rotation = checkPoints[num].rotation;
        
        if(spawn != null && spawn.spawnPos != null)
            spawn.spawnPos = checkPoints[num];

        if (isAdditiveLevelLoadEnabled)
        {
            string sceneNameToLookFor = checkPoints[num].gameObject.scene.name;            
            Debug.Log("Checkpoint belongs to scene name: " + sceneNameToLookFor);
            //GameObject scenePrefabObject = GameObject.Find(sceneNameToLookFor);

            for (int i=0; i < SceneManager.sceneCount; i++)
            {
                GameObject scenePrefabObject = GameObject.Find(SceneManager.GetSceneAt(i).name);

                if (scenePrefabObject != null)
                {
                    Debug.Log("Scene prefab object wasn't null!");
                    if (scenePrefabObject.TryGetComponent<ScenePartLoader>(out ScenePartLoader scenePartLoaderComp))
                    {
                        if (scenePrefabObject.name == sceneNameToLookFor)
                        {
                            scenePartLoaderComp.shouldLoad = true;
                            scenePartLoaderComp.LoadScene();      
                        }  
                        else
                        {
                            scenePartLoaderComp.shouldLoad = false;
                            scenePartLoaderComp.UnLoadScene();
                        }                                    
                    }                  
                }               
            }
            
            //Debug.Log(scenePrefabObject);
            
            /*    
                // If checkpoint belongs to this scene, load scene associated with it
                if (SceneManager.GetSceneAt(i) == checkPoints[num].gameObject.scene)
                //SceneManager.GetSceneByName
                {
                    GameObject scenePrefabObject = GameObject.Find(SceneManager.GetSceneAt(i).name);
                    if (scenePrefabObject != null)
                    {
                        if (scenePrefabObject.TryGetComponent<ScenePartLoader>(out ScenePartLoader scenePartLoaderComp))
                        {
                            scenePartLoaderComp.LoadScene();
                        }
                    }
                }
            */
        }
        
        //if (checkPoints[num].gameObject.scene)

        



        
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

    public void ToggleHUD()
    {
    }
}
