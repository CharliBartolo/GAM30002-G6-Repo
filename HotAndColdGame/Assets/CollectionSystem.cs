using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


/// <summary>
/// Responsible for managing collectibles, such as journals, the raygun etc.
/// Manages their found status, whether they should or shouldn't appear on level load,
/// accessing any associated data (such as journal entries).
/// Last edit: Expanded journals to be indexed by scene number, allowed collectibles
///            to be correctly indexed when revisiting previous level.
/// By: Charli - 30/10/21
/// </summary>
public class CollectionSystem : MonoBehaviour
{
    // add parent gameobject containing colectable items here
    [SerializeField] public GameObject Collectables;
    // collections UI object
    private Transform ui_collection;

    // static variables
    // static list of LevelData
    //public static Dictionary<string, LevelData> levelList;
    // list access variable
    public Dictionary<int, LevelData> levelList;

    //public static List<JournalPage> AllJournals;
    // allJournals as indexed by scene index
    //public Dictionary<int, JournalPageCollection> allJournalsWithSceneIndex;
    public Dictionary<int, List<JournalPage>> allJournalsWithSceneIndex;
    //private int journalSceneIndex = 0;
    // started flag
    protected bool started;
    // levels count
    protected int levelsCount = 0;
    // journal id 
    int journalId = 0;
    int artifactId = 0;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded");
        Initialise();
    }

    // Start is called before the first frame update
    void Start()
    {
       SceneManager.sceneLoaded += OnSceneLoaded;
       Initialise();
    }

    public void Initialise()
    {
        // initialise level list if not started
        if (!started)
        {
            //allJournalsWithSceneIndex = new Dictionary<int, JournalPageCollection>();
            allJournalsWithSceneIndex = new Dictionary<int, List<JournalPage>>();
            //journalSceneIndex = 0;
            levelList = new Dictionary<int, LevelData>();
            started = true;

            //LevelList = levelList;
            //allJournals = AllJournals;
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
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        int logNumber = 1;
        journalId = 0;
        artifactId = 0;

        CollectInteractable[] collectables = Collectables.GetComponentsInChildren<CollectInteractable>();
        //CollectInteractable[] collectables = FindObjectsOfType<CollectInteractable>();
        List<CollectInteractable> Artifacts = new List<CollectInteractable>();

        //if (!levelList.ContainsKey(levelName))
        if (!levelList.ContainsKey(levelIndex))
        {
            print("Level List does not contain this level's data! Adding...");
            List<JournalPage> Journals = new List<JournalPage>();
           
          
            // add collectables to their lists
            for (int i = 0; i < collectables.Length; i++)
            {
                if (collectables[i].gameObject.activeSelf == true)
                {
                    //collectables[i].int_data = i;

                    if (collectables[i].itemName == "Journal")
                    {
                        collectables[i].int_data = journalId;
                        JournalPage page = new JournalPage(collectables[i].GetComponent<Journal>().EntryLog[0], collectables[i].GetComponent<Journal>().EntryLog[1], journalId);
                        Journals.Add(page);
                        //allJournalsWithSceneIndex.Add
                        //allJournals.Insert(journalId, page);
                        
                        journalId++;
                      
                    }
                    else if (collectables[i].itemName.Contains("Artifact") || collectables[i].itemName.Contains("Raygun"))
                    {
                        collectables[i].int_data2 = artifactId;
                        Artifacts.Add(collectables[i]);
                        artifactId++;
                    }
                }
            }

            // append data to text entries
            for (int i = 0; i < Journals.Count; i++)
            {
                string pg1_Numbered = levelName + " - " + "Log " + logNumber.ToString() + " of " + Journals.Count + '\n' + '\n' + Journals[i].text[0];
                Journals[i].text[0] = pg1_Numbered;
                logNumber++;
            }

            LevelData levelData = new LevelData(levelName, levelIndex, Journals, Artifacts);
            allJournalsWithSceneIndex.Add(levelIndex, Journals);


            // add level to list
            levelList.Add(levelData.levelIndex, levelData);

            Debug.Log("LEVEL DATA ADDED");
        }
        else
        {
            // Remap IDs to logs in level
            for (int i = 0; i < collectables.Length; i++)
            {
                if (collectables[i].gameObject.activeSelf == true)
                {
                    if (collectables[i].itemName == "Journal")
                    {
                        collectables[i].int_data = journalId;                        
                        journalId++;                      
                    }

                    else if (collectables[i].itemName.Contains("Artifact") || collectables[i].itemName == "Raygun")
                    {
                        collectables[i].int_data2 = artifactId;
                        artifactId++;
                    }

                }
            }
            GameMaster.instance.CheckAlreadyFoundCollectibles();
        }

    }

    
    public JournalPage RetrieveFromAllJournals(int id)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        return allJournalsWithSceneIndex[currentSceneIndex][id];
        //return allJournals[id];
    }
    
    public List<JournalPage> RetrieveFoundPages()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        List<JournalPage> allFoundJournals = new List<JournalPage>();
        //return allJournals.Where(x => x.found == true).ToList();
        foreach (List<JournalPage> setOfJournals in allJournalsWithSceneIndex.Values)
        {
            allFoundJournals.AddRange(setOfJournals.Where(x => x.found == true));
        }   
        
        return allFoundJournals;
        //return allJournalsWithSceneIndex[currentSceneIndex].Where(x => x.found == true).ToList();
    }

    // retrieve a specific journal page
    //public JournalPage RetrieveJournalPage(string levelName, int id)
    public JournalPage RetrieveJournalPage(int levelIndex, int id)
    {
        JournalPage page = null;

        page = levelList[levelIndex].Journals[id];

        return page;

    }

    // retrieve next found journal page from AllJournals list
    public JournalPage RetrieveNextFoundPage()
    {
        JournalPage page = null;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        foreach (List<JournalPage> setOfJournals in allJournalsWithSceneIndex.Values)
        {
            //page = allJournalsWithSceneIndex[currentSceneIndex].SkipWhile(x => x.found != true).Skip(0).FirstOrDefault();
            page = setOfJournals.SkipWhile(x => x.found != true).Skip(0).FirstOrDefault();
        }       

        return page;
    }


    // retrieve previous found journal page from AllJournals list
    public JournalPage RetrievePreviousFoundPage()
    {
        JournalPage page = null;

        foreach (List<JournalPage> setOfJournals in allJournalsWithSceneIndex.Values)
        {
            page = setOfJournals.TakeWhile(x => x.found != true).DefaultIfEmpty(setOfJournals[setOfJournals.Count - 1]).LastOrDefault();
            //page = allJournals.TakeWhile(x => x.found != true).DefaultIfEmpty(allJournals[allJournals.Count - 1]).LastOrDefault();
        }              

        return page;
    }

    // set found to true for item leveldata when item found
    public void FoundCollectable(CollectInteractable item)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (item.itemName == "Journal")
        {
            //allJournalsWithSceneIndex[item.int_data].found = true;
            allJournalsWithSceneIndex[currentSceneIndex][item.int_data].found = true;
            //levelList[SceneManager.GetActiveScene().name].Journals[item.int_data].found = true;
        }
        else if (item.itemName.Contains("Artifact") || item.itemName.Contains("Raygun"))
        {
            levelList[SceneManager.GetActiveScene().buildIndex].Artifacts[item.int_data2] = true;
        }

        UpdateUI();

        //activate popup
        //ui_collection.GetComponentInParent<Journal_Reader>().DisplayPopup();
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
            results[i] = (levelList.ElementAt(i).Value.JournalsFound == levelList.ElementAt(i).Value.Journals.Count) && (levelList.ElementAt(i).Value.ArtifactsFound == levelList.ElementAt(i).Value.Artifacts.Count);
            Debug.Log("RESULTS:" + results);
        }

        return results.ToList();
    }

    // struct for holding level collectables data
    public struct LevelData
    {
        public string levelName;
        public int levelIndex;
        public SortedDictionary<int, JournalPage> Journals;
        //public Dictionary<int, bool> Artifacts;
        public Dictionary<int, bool> Artifacts;


        public LevelData(string levelName, int levelIndex, List<JournalPage> Journals, List<CollectInteractable> Artifacts)
        {
            this.levelName = levelName;
            this.levelIndex = levelIndex;
            this.Journals = new SortedDictionary<int, JournalPage>();
            //this.Artifacts = new Dictionary<int, bool>();
            this.Artifacts = new Dictionary<int, bool>();

            // create collections and assign false collected valued
            AddCollectables(Journals, Artifacts);
        }


        // adds collectables to their lists
        void AddCollectables(List<JournalPage> journals, List<CollectInteractable> artifacts)
        {
            foreach (JournalPage page in journals)
            {
                this.Journals.Add(page.id, page);
            }

            foreach (CollectInteractable item in artifacts)
            {
                this.Artifacts.Add(item.int_data2, false);
                //this.Artifacts.Add(item.int_data, false);
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
        public int CountArtifactsFound(Dictionary<int, bool> collection)
        //public int CountArtifactsFound(Dictionary<int, bool> collection)
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
}

public class JournalPageCollection
{
    public List<JournalPage> journalPages = new List<JournalPage>();
}

