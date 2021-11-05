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
/// Last edit: Allow player to toggle between readable font and current text font.
/// By: Jason - 3/11/21
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
    public Font readingFont;
    private Font temp;

    public float timeoutDisplay = 2f;
    private bool toggle = false;

    // input variables
    private PlayerInput playerInput;
    private Vector2 input = Vector2.zero;

    public bool isJournalActive = false;
    public static int currentPage = 0;

    private List<JournalPage> pagesToRead;


    public bool newPageAdded;
    public AudioClip scribeSound;
    public AudioClip pageTurnSound;

    //artifact icon sprite array
    public Sprite[] artifactIcons;

    private void Start()
    {
        // initialise pages list
        levelPages = new List<JournalPage>();
        // initialise pages to null

        FindPlayerReference();
        //if (playerInput != null)
        //    playerInput.actions.FindActionMap("Menu").FindAction("ToggleReadable").performed += ToggleReadable;
       

        homePage = transform.Find("HomePage");
        popUp = transform.parent.Find("Popup");
        DisplayHomePage(false);
        Exit_Journal();


        isJournalActive = false;
    }

    private void Update()
    {
        if(isJournalActive)
        {
            if (playerInput != null)
            {
                ReadInput();
            }
            else
            {
                // find player input controller
               FindPlayerReference();
            }
                
        }
    }

    private void FindPlayerReference()
    {
        if (GameMaster.instance != null && GameMaster.instance.playerRef != null)
                    playerInput = GameMaster.instance.playerRef.GetComponent<PlayerInput>();
    }

    private void ToggleReadable(InputAction.CallbackContext context)
    {
        toggle = !toggle;

        if (toggle)
        {
            temp = text[0].font;
            temp = text[1].font;
            text[0].font = readingFont;
            text[1].font = readingFont;
        }
        else 
        {
            text[0].font = temp;
            text[1].font = temp;
        }
    }

    void ReadInput()
    {
        Debug.Log("INPUT: " + playerInput.actions.FindActionMap("Menu").FindAction("Navigate").ReadValue<Vector2>().x);
        
        // get  input for journal navigation
        if (input.x != playerInput.actions.FindActionMap("Menu").FindAction("Navigate").ReadValue<Vector2>().x)
        {
            input.x = playerInput.actions.FindActionMap("Menu").FindAction("Navigate").ReadValue<Vector2>().x;
            Debug.Log("Journal input x: " + input.x);
            NavigateJournal((int)input.x);
            Debug.Log("Current page: " + currentPage);
        }
        // toggle between current font and readable font
        
        //if (Input.GetKeyDown(KeyCode.T)) 
        if (playerInput.actions.FindActionMap("Menu").FindAction("ToggleReadable").triggered)
        {
            toggle = !toggle;

            if (toggle)
            {
                temp = text[0].font;
                temp = text[1].font;
                text[0].font = readingFont;
                text[1].font = readingFont;
            }
            else 
            {
                text[0].font = temp;
                text[1].font = temp;
            }
        }
        
        /*
        // get  input for journal navigation
        if (input.x != playerInput.actions.FindAction("Navigate").ReadValue<Vector2>().x)
        {
            input.x = playerInput.actions.FindAction("Navigate").ReadValue<Vector2>().x;
            Debug.Log("Journal input x: " + input.x);
            NavigateJournal((int)input.x);
            Debug.Log("Current page: " + currentPage);
        }*/
    }


    public void NavigateJournal(int dir)
    {
        //Debug.Log("LEVEL NAME :" + GameMaster.instance.GetComponent<CollectionSystem>().LevelList[SceneManager.GetActiveScene().name].levelName + "  Journals: " + GameMaster.instance.GetComponent<CollectionSystem>().LevelList[SceneManager.GetActiveScene().name].Journals.Count);
        
        if(pagesToRead.Count > 0)
        {
            if (dir > 0)
            {
                if (currentPage < pagesToRead.Count)
                {
                    DisplayHomePage(false);
                    currentPage++;
                    JournalPage page = pagesToRead[currentPage-1];


                    if (page != null)
                        Display_Journal(page.text[0], page.text[1], 0);
                }

            }
            else if (dir < 0)
            {
                if (currentPage > 1)
                {
                    //DisplayHomePage(false);
                    currentPage--;
                    

                    JournalPage page = pagesToRead[currentPage - 1];

                    if (page != null)
                        Display_Journal(page.text[0], page.text[1], 0);
                }
                else
                {
                    if (homePage.gameObject.activeSelf == false)
                    {
                        DisplayHomePage(true);
                        currentPage = 0;
                    }
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

    public void DisplayPopup(string type)
    {
        popUp.gameObject.SetActive(true);
        popUp.transform.GetChild(0).gameObject.SetActive(false);
        popUp.transform.GetChild(1).gameObject.SetActive(true);
        switch (type)
        {
            case "Journal":
                popUp.transform.GetChild(1).gameObject.SetActive(false);
                popUp.transform.GetChild(0).gameObject.SetActive(true);
                break;

            case "Raygun":
                popUp.transform.GetChild(1).GetComponent<Image>().sprite = artifactIcons[0];
                break;

            case "Globe":
                popUp.transform.GetChild(1).GetComponent<Image>().sprite = artifactIcons[4];
                break;

            case "Specimen":
                popUp.transform.GetChild(1).GetComponent<Image>().sprite = artifactIcons[2];
                break;

            case "Skull":
                popUp.transform.GetChild(1).GetComponent<Image>().sprite = artifactIcons[1];
                break;

            case "Helmet":
                popUp.transform.GetChild(1).GetComponent<Image>().sprite = artifactIcons[3];
                break;
        }
       
    }

    public void Display_Journal(string journal_text_pg1, string journal_text_pg2, int journalType)
    {
        if(!isJournalActive)
        {
            isJournalActive = true;
            Background_Human.gameObject.SetActive(true);
            pagesToRead = GameMaster.instance.GetComponent<CollectionSystem>().RetrieveFoundPages();
            Debug.Log("PAGES TO READ: " + pagesToRead.Count);
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

            Camera.main.GetComponent<AudioSource>().PlayOneShot(pageTurnSound);

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
        currentPage = 0;
        text[0].gameObject.SetActive(false);
        text[1].gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        Background_Human.SetActive(false);
        Background_Alien.SetActive(false);

        DisplayHomePage(false);

        if (isJournalActive)
        {
            isJournalActive = false;
            if(newPageAdded)
            {
                DisplayPopup("Journal");
                newPageAdded = false;
                Camera.main.GetComponent<AudioSource>().PlayOneShot(scribeSound);
            }
           
        }
    }

    public void SetTimeout(float time)
    {
        Invoke(nameof(Exit_Journal), time);
    }

    private void OnDestroy() 
    {
        //playerInput.actions.FindActionMap("Menu").FindAction("ToggleReadable").performed -= ToggleReadable;
    }

}

// struct for journal page data
public class JournalPage
{
    public List<string> text;
    public int id;
    public bool found;

    public JournalPage(string page1Text, string page2Text, int id, bool found = false)
    {
        this.text = new List<string>();
        this.text.Add(page1Text);
        this.text.Add(page2Text);
        this.id = id;
        this.found = found;
    }
}