using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

// struct for holding level collectables data
public struct LevelData
{
    public string levelName;
    public Dictionary<CollectInteractable, bool> Journals;
    public Dictionary<CollectInteractable, bool> Artifacts;


    public LevelData(string levelName, List<CollectInteractable> Journals, List<CollectInteractable> Artifacts)
    {
        this.levelName = levelName;
        this.Journals = new Dictionary<CollectInteractable, bool>();
        this.Artifacts = new Dictionary<CollectInteractable, bool>();


        // create collections and assign false collected valued
        AddCollectables(Journals, Artifacts);
    }


    // adds collectables to their lists
    void AddCollectables(List<CollectInteractable> journals, List<CollectInteractable> artifacts)
    {
        foreach (var item in journals)
        {
            CollectInteractable journalCopy = item;
            this.Journals.Add(item, false);
        }

        foreach (var item in artifacts)
        {
            this.Artifacts.Add(item, false);
        }
    }

    // return number of Journals found
    public int JournalsFound => NumberFound(Journals);

    // return number of Artifacts found
    public int ArtifactsFound => NumberFound(Artifacts);

    // find number of found items, indicated by a 'true' status, in a collection
    public int NumberFound(Dictionary<CollectInteractable, bool> collection)
    {
        int result = 0;

        foreach (var item in collection.Values)
        {
            if (item == true)
                result++;
        }

        return result;
    }

}

public class CollectionSystem : MonoBehaviour
{
    

    // add parent gameobject containing colectable items here
    [SerializeField] public GameObject Collectables;
    // collections UI object
    private Transform ui_collection;

    // static variables
    // list of LevelData
    public static Dictionary<string, LevelData> levelList;

    public Dictionary<string, LevelData> LevelList;
    // started flag
    protected static bool started;
    // levels count
    protected static int levelsCount = 0;

    private void Awake()
    {
        //Debug.Log("Awake");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded");
        Initialise();
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelList = levelList;
    }

    public void Initialise()
    {
        // initialise level list if not started
        if (!started)
        {
            levelList = new Dictionary<string, LevelData>();
            started = true;
            Debug.Log("LIST INITIALISED!");
        }

        // Find "Collectables" gameObject in scene and assign it 
        Collectables = GameObject.Find("Collectables");

        // find Journal Reader 
        ui_collection = FindCollectionsUI();

        if (ui_collection != null && Collectables != null)
        {
            //Debug.Log("Collectables found in scnene, adding level to master");

            // update lists and UI
            //PopulateLists();
            AddLevelToMaster();
            PopulateUI();
            UpdateUI();

            // increment level count

        }
        else
        {
            Debug.Log("No collectables in scnene");
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    // add level to master list
    public void AddLevelToMaster()
    {
        string levelName = SceneManager.GetActiveScene().name;
        if (!levelList.ContainsKey(levelName))
        {
            List<CollectInteractable> Journals = new List<CollectInteractable>();
            List<CollectInteractable> Artifacts = new List<CollectInteractable>();

            CollectInteractable[] collectables = Collectables.GetComponentsInChildren<CollectInteractable>();

            // add collectables to their lists
            foreach (var item in Collectables.GetComponentsInChildren<CollectInteractable>())
            {
                if (item.gameObject.activeSelf == true)
                {
                    if (item.itemName == "Journal")
                    {
                        Journals.Add(item);
                    }
                    else if (item.itemName == "Artifact")
                    {
                        Artifacts.Add(item);
                    }
                }
            }

            // add int data as index to each list item (so that artifacts start from zero also)
            // journals int data
            for (int i = 0; i < Journals.Count; i++)
            {
                Journals[i].int_data = i;
            }
            // artifacts int data
            for (int i = 0; i < Artifacts.Count; i++)
            {
                Artifacts[i].int_data = i;
            }

            // create level data

            LevelData data = new LevelData(levelName, Journals, Artifacts);

            // add level to list
            levelList.Add(data.levelName, data);

            Debug.Log("LEVEL DATA ADDED");
        }

    }

    // set found to true for item leveldata when item found
    public void FoundCollectable(CollectInteractable item)
    {
        if (item.itemName == "Journal")
        {

            levelList[SceneManager.GetActiveScene().name].Journals[item] = true;
        }
        else if (item.itemName == "Artifact")
        {
            levelList[SceneManager.GetActiveScene().name].Artifacts[item] = true;
        }

        UpdateUI();

        //activate popup
        ui_collection.GetComponentInParent<Journal_Reader>().DisplayPopup();
    }

    // find the Collections / Journal Reader
    public Transform FindCollectionsUI()
    {
        if (GameObject.Find("JournalReader") != null)
        {
            return GameObject.Find("JournalReader").transform.Find("HomePage");
        }
        return null;
    }

    // populate UI
    public void PopulateUI()
    {

        // get level name (using scene name for now)
        //ui_collection.Find("LevelName").GetComponent<Text>().text = SceneManager.GetActiveScene().name;

        //ui_collection.Find("LevelName").GetComponent<Text>().text = levelList[SceneManager.GetActiveScene().name].levelName;



        // update level names
        Text[] levelNames = ui_collection.Find("Level Names").GetComponentsInChildren<Text>();

        for (int i = 0; i < levelList.Count; i++)
        {
            levelNames[i].text = levelList.ElementAt(i).Value.levelName;

        }
    }

    // update UI
    public void UpdateUI()
    {
        // update journal count
        ui_collection.Find("Collections_Journal").transform.Find("Count").GetComponent<Text>().text = levelList[SceneManager.GetActiveScene().name].JournalsFound.ToString() + " / " + levelList[SceneManager.GetActiveScene().name].Journals.Count.ToString();

        // update artifact count
        ui_collection.Find("Collections_Artifacts").transform.Find("Count").GetComponent<Text>().text = levelList[SceneManager.GetActiveScene().name].ArtifactsFound.ToString() + " / " + levelList[SceneManager.GetActiveScene().name].Artifacts.Count.ToString();
    }
}
