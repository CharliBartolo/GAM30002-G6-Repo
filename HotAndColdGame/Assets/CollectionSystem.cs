using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionSystem : MonoBehaviour
{
    // add parent gameobject containing colectable items here
    [SerializeField] public GameObject Collectables;

    private List<CollectInteractable> Journals;
    private List<CollectInteractable> Artifacts;

    protected int totalCount = 0;
    protected int journalsFound = 0;
    protected int artifactsFound = 0;

    private Transform ui_collection;

    private List<int> JournalsFound;
    private List<int> ArtifactsFound;

    // stored found collectables by int data

    // Start is called before the first frame update
    void Start()
    {
        // Find "Collectables" gameObject in scene and assign it 

        Collectables = GameObject.Find("Collectables");
        // list of collectable items
        Journals = new List<CollectInteractable>();
        Artifacts = new List<CollectInteractable>();

        // list of found items int_data for reference
        JournalsFound = new List<int>();
        ArtifactsFound = new List<int>();

        ui_collection = FindCollectionsUI();

        if (ui_collection != null && Collectables != null)
        {
            PopulateLists();
            PopulateUI();
            UpdateUI();
        }

        if (Journals.Count > 0) Debug.Log("Journals: " + Journals.Count);
        if (Artifacts.Count > 0) Debug.Log("Artifacts: " + Artifacts.Count);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FoundCollectable(string name, int data)
    {
        if (name == "Journal")
        {
            if (!JournalsFound.Contains(data))
            {
                JournalsFound.Add(data);
                journalsFound++;
            }

        }
        else if (name == "Artifact")
        {
            if (!ArtifactsFound.Contains(data))
            {
                ArtifactsFound.Add(data);
                artifactsFound++;
            }
        }

        UpdateUI();

        //activate popup
        ui_collection.GetComponentInParent<Journal_Reader>().DisplayPopup();
    }

    public Transform FindCollectionsUI()
    {
        if(GameObject.Find("JournalReader") != null)
        {
            return GameObject.Find("JournalReader").transform.Find("HomePage");
        }
        return null;
    }

    public void PopulateLists()
    {
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
    }

    public void PopulateUI()
    {
        Debug.Log("Found and populating collections UI");

        // get level name (using scene name for now)
        ui_collection.Find("LevelName").GetComponent<Text>().text = SceneManager.GetActiveScene().name;
    }

    public void UpdateUI()
    {
        // update journal count
        ui_collection.Find("Collections_Journal").transform.Find("Count").GetComponent<Text>().text = journalsFound.ToString() + " / " + Journals.Count.ToString();

        // update artifact count
        ui_collection.Find("Collections_Artifacts").transform.Find("Count").GetComponent<Text>().text = artifactsFound.ToString() + " / " + Artifacts.Count.ToString();
    }

    public int TotalJournals()
    {
        int result = 0;




        return result;
    }


}
