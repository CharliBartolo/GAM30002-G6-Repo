using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// A class dedicated to viewing lecel collection items, status and reading journal entries from Journal.cs
/// by displaying them on the screen for the player.
/// Last edit: Expanded class have pages, popup displayed to on new entry, and input control for journal navigation.
/// By: Matt - 13/10/21
/// </summary>
public class Journal_Reader : MonoBehaviour
{
  

    // pages list
    public List<JournalPage> levelPages;

    // journal reader variables
    public Transform homePage;
    public Transform popUp;

    public List<Text> text;
    public Image image;
    public GameObject Background_Human;
    public GameObject Background_Alien;
    public Font font_Human;
    public Font font_Alien;

    public float timeoutDisplay = 2f;

    // input variables
    private PlayerInput playerInput;
    private Vector2 input = Vector2.zero;

    public bool isJournalActive = false;
    public static int currentPage = 0;

    private void Start()
    {
        // initialise pages list
        levelPages = new List<JournalPage>();
        // initialise pages to null


        // find player input controller
        if (GameMaster.instance != null && GameMaster.instance.playerRef != null)
            playerInput = GameMaster.instance.playerRef.GetComponent<PlayerInput>();

        homePage = transform.Find("HomePage");
        popUp = transform.Find("Popup");
        DisplayHomePage(false);
        Exit_Journal();


        isJournalActive = false;
    }

    private void Update()
    {
        if(isJournalActive)
        {
            if (playerInput != null)
                ReadInput();
        }
    }

    void ReadInput()
    {
        // get  input for journal navigation
        if (input.x != playerInput.actions.FindAction("Movement").ReadValue<Vector2>().x)
        {
            input.x = playerInput.actions.FindAction("Movement").ReadValue<Vector2>().x;
            Debug.Log("Journal input x: " + input.x);
            NavigateJournal((int)input.x);
            Debug.Log("Current page: " + currentPage);
        }
    }

    public void NavigateJournal(int dir)
    {
        if (GameMaster.instance.GetComponent<CollectionSystem>().LevelList[SceneManager.GetActiveScene().name].JournalsFound > 0)
        {
            if(dir > 0)
            {
                if(currentPage <= GameMaster.instance.GetComponent<CollectionSystem>().LevelList[SceneManager.GetActiveScene().name].Journals.Count)
                {
                    DisplayHomePage(false);
                    currentPage++;

                    Journal journal = GameMaster.instance.GetComponent<CollectionSystem>().LevelList[SceneManager.GetActiveScene().name].Journals.Keys.ElementAt(currentPage-1).GetComponent<Journal>();

                    Display_Journal(journal.EntryLog[0], journal.EntryLog[1], 0);
                }
            }
            else if (dir < 0)
            {
                if(homePage.gameObject.activeSelf == false)
                {
                    DisplayHomePage(true);
                    currentPage = 0;
                }
            }
        }
    }

    // display or hide the journal reader's home page elements
    public void DisplayHomePage(bool option)
    {
        // do extra stuff for option
        if (option == true)
        {
            // hide pages textbox
            text[0].gameObject.SetActive(false);
            text[1].gameObject.SetActive(false);
        }
        else if(option == false)
        {
           
        }
        // set homepage active state to option
        homePage.gameObject.SetActive(option);
    }

    public void DisplayPopup()
    {
        popUp.gameObject.SetActive(true);
    }

    public void AddNewPage()
    {

    }

    public void Display_Journal(string journal_text_pg1, string journal_text_pg2, int journalType)
    {
        if(!isJournalActive)
        {
            isJournalActive = true;
            Background_Human.gameObject.SetActive(true);
        }
       
        // display homepage if jounalType is -1, else show page
        if (journalType == -1)
        {
            DisplayHomePage(true);

        }
        else
        {
            text[0].text = journal_text_pg1;
            //textbox.SetActive(true);
            text[0].gameObject.SetActive(true);

            text[1].text = journal_text_pg2;
            //textbox.SetActive(true);
            text[1].gameObject.SetActive(true);
            //image.gameObject.SetActive(true);

            //SetTimeout(timeoutDisplay);
       
            DisplayHomePage(false);

            if (journalType == 0)
            {
                Background_Human.gameObject.SetActive(true);
                text[0].font = font_Human;
                text[1].font = font_Human;
            }
            else if (journalType == 1)
            {
                Background_Alien.gameObject.SetActive(true);
                text[0].font = font_Alien;
                text[1].font = font_Alien;
            }
        }
    }

    public void Exit_Journal()
    {
        //text.GetComponent<Text>().text = journal_text;
        //textbox.SetActive(false);
        text[0].gameObject.SetActive(false);
        text[1].gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        Background_Human.SetActive(false);
        Background_Alien.SetActive(false);

        DisplayHomePage(false);

        if (isJournalActive)
        {
            isJournalActive = false;
        }
    }

    public void SetTimeout(float time)
    {
        Invoke(nameof(Exit_Journal), time);
    }

   
}

// struct for journal page data
public struct JournalPage
{
    public List<string> text;

    public JournalPage(string page1Text, string page2Text)
    {
        text = new List<string>();
        text.Add(page1Text);
        text.Add(page2Text);
    }
}