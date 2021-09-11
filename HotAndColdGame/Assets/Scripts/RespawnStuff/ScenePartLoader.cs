using UnityEngine;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger
}
public class ScenePartLoader : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public CheckMethod checkMethod;
    [SerializeField] public float loadRange;
    [SerializeField] public bool meshOnly;

    //Scene state
    private bool isLoaded;
    private bool shouldLoad;
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
            TriggerCheck();
        }
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

    void LoadScene()
    {
        if (!isLoaded)
        {
            //Loading the scene, using the gameobject name as it's the same as the name of the scene to load
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            //We set it to true to avoid loading the scene twice
            isLoaded = true;
        }
        //need to add function to turn on render
    }

    void UnLoadScene()
    {
        //original code
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
        //need to add function to turn off render
    }

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
    }

    void TriggerCheck()
    {
        //shouldLoad is set from the Trigger methods
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

