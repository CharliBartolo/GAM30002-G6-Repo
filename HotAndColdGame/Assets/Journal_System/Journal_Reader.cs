using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal_Reader : MonoBehaviour
{
    //public GameObject textbox;
    public Text text;
    public Image image;
    public GameObject Background_Human;
    public GameObject Background_Alien;

    public float timeoutDisplay = 2f;

    private void Start()
    {
        Exit_Journal();
    }

    public void Display_Journal(string journal_text, int journalType)
    {
        text.text = journal_text;
        //textbox.SetActive(true);
        text.gameObject.SetActive(true);
        //image.gameObject.SetActive(true);

        SetTimeout(timeoutDisplay);

        if (journalType == 0)
        {
            Background_Human.gameObject.SetActive(true);
        }
        else
        {
            Background_Alien.gameObject.SetActive(true);
        }
    }
    public void Exit_Journal()
    {
        //text.GetComponent<Text>().text = journal_text;
        //textbox.SetActive(false);
        text.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        Background_Human.SetActive(false);
        Background_Alien.SetActive(false);
    }

    public void SetTimeout(float time)
    {
        Invoke(nameof(Exit_Journal), time);
    }
}
