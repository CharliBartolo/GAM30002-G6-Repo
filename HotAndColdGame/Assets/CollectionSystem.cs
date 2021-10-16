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
    public SortedDictionary<int, JournalPage> Journals;
    public Dictionary<CollectInteractable, bool> Artifacts;


    public LevelData(string levelName, List<JournalPage> Journals, List<CollectInteractable> Artifacts)
    {
        this.levelName = levelName;
        this.Journals = new SortedDictionary<int, JournalPage>();
        this.Artifacts = new Dictionary<CollectInteractable, bool>();


        // create collections and assign false collected valued
        AddCollectables(Journals, Artifacts);
    }


    // adds collectables to their lists
    void AddCollectables(List<JournalPage> journals, List<CollectInteractable> artifacts)
    {
        foreach (var page in journals)
        {
            this.Journals.Add(page.id, page);
        }

        foreach (var item in artifacts)
        {
            this.Artifacts.Add(item, false);
        }
    }

    // return number of Journals found
    public int JournalsFound => CountJournalsFound(Journals);

    // return number of Artifacts found
    public int ArtifactsFound => CountArtifactsFound(Artifacts);

    // find number of journals found
    public int CountJournalsFound(SortedDictionary<int, JournalPage> collection)
    {
        int result = 0;


        foreach (var item in collection.Values)
        {
            if (item.found == true)
                result++;
        }

        return result;
    }

    // find number of found items, indicated by a 'true' status, in a collection
    public int CountArtifactsFound(Dictionary<CollectInteractable, bool> collection)
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
    // static list of LevelData
    public static Dictionary<string, LevelData> levelList;
    // list access variable
    public  Dictionary<string, LevelData> LevelList;

    public static List<JournalPage> AllJournals;
    public List<JournalPage> allJournals;
    // started flag
    protected static bool started;
    // levels count
    protected static int levelsCount = 0;
    // journal id 
    static int journalId = 0;

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
       
    }

    public void Initialise()
    {
        // initialise level list if not started
        if (!started)
        {
            AllJournals = new List<JournalPage>();
            levelList = new Dictionary<string, LevelData>();
            started = true;

            LevelList = levelList;
            allJournals = AllJournals;
            //Debug.Log("LIST INITIALISED!");
        }

        // Find "Collectables" gameObject in scene and assign it 
        Collectables = GameObject.Find("Collectables");

        // find Journal Reader 
        ui_collection = FindCollectionsUI();

        if (ui_collection != null && Collectables != null)
        {
            // update lists and UI
            AddLevelToMaster();
            UpdateUI();

            //Debug.Log("ALL JOURNALS LENGTH: " + AllJournals.Count);
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
        int logNumber = 1;


        if (!levelList.ContainsKey(levelName))
        {
            List<JournalPage> Journals = new List<JournalPage>();
            List<CollectInteractable> Artifacts = new List<CollectInteractable>();

            CollectInteractable[] collectables = Collectables.GetComponentsInChildren<CollectInteractable>();

          
            // add collectables to their lists
            for (int i = 0; i < Collectables.GetComponentsInChildren<CollectInteractable>().Length; i++)
            {
                if (collectables[i].gameObject.activeSelf == true)
                {
                    //collectables[i].int_data = i;

                    if (collectables[i].itemName == "Journal")
                    {
                        collectables[i].int_data = journalId;
                        JournalPage page = new JournalPage(collectables[i].GetComponent<Journal>().EntryLog[0], collectables[i].GetComponent<Journal>().EntryLog[1], journalId);

                        Journals.Add(page);
                        AllJournals.Insert(journalId, page);
                        
                        journalId++;
                      
                    }
                    else if (collectables[i].itemName == "Artifact")
                    {

                        Artifacts.Add(collectables[i]);
                    }
                }

            }

            // append data to text entries
            for (int i = 0; i < Journals.Count; i++)
            {
                string pg1_Numbered = levelName + " - " + "Log " + logNumber.ToString() + " of " + Journals.Count + '\n' + Journals[i].text[0];
                Journals[i].text[0] = pg1_Numbered;
                logNumber++;
            }


            LevelData data = new LevelData(levelName, Journals, Artifacts);

            // add level to list
            levelList.Add(data.levelName, data);

            Debug.Log("LEVEL DATA ADDED");
        }

    }

    public JournalPage RetrieveFromAllJournals(int id)
    {
        return AllJournals[id];
    }

    public List<JournalPage> RetrieveFoundPages()
    {
        return AllJournals.Where(x => x.found == true).ToList();
    }

    // retrieve a specific journal page
    public JournalPage RetrieveJournalPage(string levelName, int id)
    {
        JournalPage page = null;

        page = levelList[levelName].Journals[id];

        return page;

    }

    // retrieve next found journal page from AllJournals list
    public JournalPage RetrieveNextFoundPage()
    {
        JournalPage page = null;

        page = AllJournals.SkipWhile(x => x.found != true).Skip(0).FirstOrDefault();

        return page;

    }


    // retrieve previous found journal page from AllJournals list
    public JournalPage RetrievePreviousFoundPage()
    {
        JournalPage page = null;

        page = AllJournals.TakeWhile(x => x.found != true).DefaultIfEmpty(AllJournals[AllJournals.Count - 1]).LastOrDefault();

        return page;

    }

    // set found to true for item leveldata when item found
    public void FoundCollectable(CollectInteractable item)
    {
        if (item.itemName == "Journal")
        {
            AllJournals[item.int_data].found = true;
            //levelList[SceneManager.GetActiveScene().name].Journals[item.int_data].found = true;
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


    // update UI
    public void UpdateUI()
    {
       
        Text[] levelNames = ui_collection.Find("Level_Names").GetComponentsInChildren<Text>();
        Text[] journalCounts = ui_collection.Find("Journals_Count").GetComponentsInChildren<Text>();
        Text[] artifactsCounts = ui_collection.Find("Artifacts_Count").GetComponentsInChildren<Text>();

        for (int i = 0; i < levelList.Count; i++)
        {
            // update level names
            levelNames[i].text = levelList.ElementAt(i).Value.levelName;

            // update journal count
            journalCounts[i].text = levelList.ElementAt(i).Value.JournalsFound.ToString() + " / " + levelList.ElementAt(i).Value.Journals.Count.ToString();

            // update artifact count
            artifactsCounts[i].text = levelList.ElementAt(i).Value.ArtifactsFound.ToString() + " / " + levelList.ElementAt(i).Value.Artifacts.Count.ToString();

        }
        /* // update journal count
         ui_collection.Find("Journals_Count").transform.Find("Count").GetComponent<Text>().text = levelList[SceneManager.GetActiveScene().name].JournalsFound.ToString() + " / " + levelList[SceneManager.GetActiveScene().name].Journals.Count.ToString();

         // update artifact count
         ui_collection.Find("Artifacts_Count").transform.Find("Count").GetComponent<Text>().text = levelList[SceneManager.GetActiveScene().name].ArtifactsFound.ToString() + " / " + levelList[SceneManager.GetActiveScene().name].Artifacts.Count.ToString();*/
    }


    public List<bool> Results()
    {
        List<bool> results = new List<bool>();
        for (int i = 0; i < 6; i++)
        {
            results.Add(false);
        }  
        

        for (int i = 0; i < levelList.Count; i++)
        {
            results[i] = levelList.ElementAt(i).Value.JournalsFound == levelList.ElementAt(i).Value.Journals.Count;
            Debug.Log("RESULTS:" + results);
        }

        return results.ToList();
    }
}
