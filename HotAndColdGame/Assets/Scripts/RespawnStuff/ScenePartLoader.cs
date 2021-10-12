using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger
    //Both
}
public class ScenePartLoader : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Scene sceneToLoad;
    [SerializeField] public CheckMethod checkMethod;
    [SerializeField] public float loadRange;
    [SerializeField] public bool meshOnly;
    [SerializeField] public List<OnTrigger> loadTriggers = new List<OnTrigger>();
    [SerializeField] public List<OnTrigger> unloadTriggers = new List<OnTrigger>();

    //Scene state
    private bool isLoaded;
    public bool shouldLoad;
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
                    isLoaded = true;                   
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

        // If the scene is mesh only and isn't already loaded, load it and hide it
        if (!isLoaded && meshOnly)
        {
            LoadScene();
            UnLoadScene();
        }
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
            //Debug.Log("Trigger Check running!");
            TriggerCheck();
        }
        /*
        else if (checkMethod == CheckMethod.Both)
        {
            BothTriggerAndDistanceCheck();
        }
        */
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
                gameObject.SetActive(objectActiveDict[gameObject]);                
            }

            objectActiveDict.Clear();
            isHidden = false;
            Debug.Log("Scene unhidden!");
        }
        //need to add function to turn on render
    }

    public void UnLoadScene()
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
        //Debug.Log("TriggerLoad running");

        foreach (OnTrigger loadTrigger in loadTriggers)
        {
            if (loadTrigger.Enter)
            {
                shouldLoad = true;
                Debug.Log("Level will be loaded");
                break;
            }
        }
    }

    private void TriggerUnload() 
    {
        //Debug.Log("TriggerUnLoad running");
        foreach (OnTrigger unloadTrigger in unloadTriggers)
        {
            if (unloadTrigger.Enter)
            {
                shouldLoad = false;
                //Debug.Log("Level will be unloaded");
                break;
            }
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

