using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal_Reader : MonoBehaviour
{
    public GameObject textbox;
    public Text text;
    public void Display_Journal(string journal_text)
    {
        text.text = journal_text;
        textbox.SetActive(true);
    }
    public void Exit_Journal(string journal_text)
    {
        text.GetComponent<Text>().text = journal_text;
        textbox.SetActive(false);
    }
}
