using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionsSystem : MonoBehaviour
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
        // list of collectable items
        Journals = new List<CollectInteractable>();
        Artifacts = new List<CollectInteractable>();

        // list of found items int_data for reference
        JournalsFound = new List<int>();
        ArtifactsFound = new List<int>();

        ui_collection = FindCollectionsUI();
       
        if (ui_collection != null)
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
        if(name == "Journal")
        {
            if(!JournalsFound.Contains(data))
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
    }

    public Transform FindCollectionsUI()
    {
        return GameObject.Find("Collections_UI").transform.Find("Collections_Items");
    }

    public void PopulateLists()
    {
        foreach (var item in Collectables.GetComponentsInChildren<CollectInteractable>())
        {
            if(item.itemName == "Journal")
            {
                Journals.Add(item);
            }
            else if (item.itemName == "Artifact")
            {
                Artifacts.Add(item);
            }
        }
    }

    public void PopulateUI()
    {
        Debug.Log("Found and populating collections UI");

        // get level name (using scene name for now)
        ui_collection.Find("Text_LevelName").GetComponent<Text>().text = SceneManager.GetActiveScene().name;
    }

    public void UpdateUI()
    {
        // update journal count
        ui_collection.Find("Collections_Journal").transform.Find("Count").GetComponent<Text>().text = journalsFound.ToString() + " / " + Journals.Count.ToString();

        // update artifact count
        ui_collection.Find("Collections_Artifact").transform.Find("Count").GetComponent<Text>().text = artifactsFound.ToString() + " / " + Artifacts.Count.ToString();
    }


}
