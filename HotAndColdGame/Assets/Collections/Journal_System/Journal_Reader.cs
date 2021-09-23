using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class dedicated to reading journal entries from Journal.cs
/// and displaying them on the screen for the player.
/// Last edit: Expanded _entryLog to be a list of strings titled _entryLogs.
/// By: Charli - 23/9/21
/// </summary>
public class Journal_Reader : MonoBehaviour
{
    //public GameObject textbox;
    public List<Text> text;
    public Image image;
    public GameObject Background_Human;
    public GameObject Background_Alien;
    public Font font_Human;
    public Font font_Alien;

    public float timeoutDisplay = 2f;

    private void Start()
    {
        Exit_Journal();
    }

    public void Display_Journal(string journal_text_pg1, string journal_text_pg2, int journalType)
    { 
        text[0].text = journal_text_pg1;
        //textbox.SetActive(true);
        text[0].gameObject.SetActive(true);

        text[1].text = journal_text_pg2;
        //textbox.SetActive(true);
        text[1].gameObject.SetActive(true);
        //image.gameObject.SetActive(true);

        SetTimeout(timeoutDisplay);

        if (journalType == 0)
        {
            Background_Human.gameObject.SetActive(true);
            text[0].font = font_Human;
            text[1].font = font_Human;
        }
        else
        {
            Background_Alien.gameObject.SetActive(true);
            text[0].font = font_Alien;
            text[1].font = font_Alien;
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
    }

    public void SetTimeout(float time)
    {
        Invoke(nameof(Exit_Journal), time);
    }
}
