using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger
<<<<<<< HEAD
    //Both
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
}
public class ScenePartLoader : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Scene sceneToLoad;
    [SerializeField] public CheckMethod checkMethod;
    [SerializeField] public float loadRange;
    [SerializeField] public bool meshOnly;
<<<<<<< HEAD
    [SerializeField] public List<OnTrigger> loadTriggers = new List<OnTrigger>();
    [SerializeField] public List<OnTrigger> unloadTriggers = new List<OnTrigger>();

    //Scene state
    private bool isLoaded;
    public bool shouldLoad;
=======
    [SerializeField] public OnTrigger loadTrigger;
    [SerializeField] public OnTrigger unloadTrigger;

    //Scene state
    private bool isLoaded;
    private bool shouldLoad;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    private bool isHidden;
    private Dictionary<GameObject, bool> objectActiveDict = new Dictionary<GameObject, bool>();

    void Start()
    {
        //verify if the scene is already open to avoid opening a scene twice
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
<<<<<<< HEAD
                    isLoaded = true;                   
=======
                    isLoaded = true;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
                    //added 8-9-21
                    /*GameObject[] gameObjs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
                    foreach (GameObject go in gameObjs)
                    {
                        if (go.hideFlags == HideFlags.HideInInspector)
                        {
                            go.hideFlags = 0;
                        }
                    }*/
                }
            }
        }
<<<<<<< HEAD

        // If the scene is mesh only and isn't already loaded, load it and hide it
        if (!isLoaded && meshOnly)
        {
            LoadScene();
            UnLoadScene();
        }
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }

    void Update()
    {
        //Checking which method to use
        if (checkMethod == CheckMethod.Distance)
        {
            DistanceCheck();
        }
        else if (checkMethod == CheckMethod.Trigger)
        {
<<<<<<< HEAD
            //Debug.Log("Trigger Check running!");
            TriggerCheck();
        }
        /*
        else if (checkMethod == CheckMethod.Both)
        {
            BothTriggerAndDistanceCheck();
        }
        */
=======
            TriggerCheck();
        }
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }

    void DistanceCheck()
    {
        //Checking if the player is within the range
        if (Vector3.Distance(player.position, transform.position) < loadRange)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }

<<<<<<< HEAD
    void BothTriggerAndDistanceCheck()
    {
        TriggerCheck();

        if (isLoaded)
        {
            // If scene is loaded, unload if very far
            foreach (OnTrigger trigger in unloadTriggers)
            {
                bool unload = true;
                if (Vector3.Distance(player.position, trigger.gameObject.transform.position) < loadRange)
                {
                   unload = false;
                }

                if (unload)
                {
                    UnLoadScene();
                }
            } 
        }

    }

    public void LoadScene()
=======
    void LoadScene()
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    {
        if (!isLoaded)
        {
            //Loading the scene, using the gameobject name as it's the same as the name of the scene to load
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            //We set it to true to avoid loading the scene twice
            isLoaded = true;
        }

        if (meshOnly && isHidden)
        {
            foreach(GameObject gameObject in objectActiveDict.Keys)
            {
<<<<<<< HEAD
                gameObject.SetActive(objectActiveDict[gameObject]);                
=======
                gameObject.SetActive(objectActiveDict[gameObject]);
                
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
            }

            objectActiveDict.Clear();
            isHidden = false;
<<<<<<< HEAD
            Debug.Log("Scene unhidden!");
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
        }
        //need to add function to turn on render
    }

<<<<<<< HEAD
    public void UnLoadScene()
=======
    void UnLoadScene()
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    {
        //original code
        if (isLoaded)
        {
            if (meshOnly && !isHidden)
            {
                Scene sceneToUnload = SceneManager.GetSceneByName(gameObject.name);
            
                GameObject[] arrayOfGameObjects = sceneToUnload.GetRootGameObjects();
                objectActiveDict.Clear();

                foreach (GameObject gameObject in arrayOfGameObjects)
                {
                    objectActiveDict.Add(gameObject, gameObject.activeSelf);
                    gameObject.SetActive(false);
                    Debug.Log("Game Object " + gameObject.name + " has been saved and hidden!");
                }

                isHidden = true;
            }
            else if (!meshOnly)
            {
                SceneManager.UnloadSceneAsync(gameObject.name);
                isLoaded = false;
            }           
        }
        //need to add function to turn off render
    }
    //added 16-09-21
    private void TriggerLoad()
    {
<<<<<<< HEAD
        //Debug.Log("TriggerLoad running");

        foreach (OnTrigger loadTrigger in loadTriggers)
        {
            if (loadTrigger.Enter)
            {
                shouldLoad = true;
                Debug.Log("Level will be loaded");
                break;
            }
=======
        if (loadTrigger.Enter)
        {
            shouldLoad = true;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
        }
    }

    private void TriggerUnload() 
    {
<<<<<<< HEAD
        //Debug.Log("TriggerUnLoad running");
        foreach (OnTrigger unloadTrigger in unloadTriggers)
        {
            if (unloadTrigger.Enter)
            {
                shouldLoad = false;
                //Debug.Log("Level will be unloaded");
                break;
            }
=======
        if (unloadTrigger.Enter)
        {
            shouldLoad = false;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
        }
    }

    /*//commented out but kept incase needed later
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
    }*/

    void TriggerCheck()
    {
        //shouldLoad is set from the Trigger methods
        TriggerLoad();
        TriggerUnload();
        if (shouldLoad)
        {
            LoadScene();            
        }
        else
        {
            UnLoadScene();
        }
    }
}

