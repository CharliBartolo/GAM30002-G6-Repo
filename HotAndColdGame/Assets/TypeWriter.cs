using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    public Text display;

    private string narrative = "";
    private string current = "";

    public float time = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        narrative = display.text;
        StartCoroutine(TypeText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TypeText()
    {
        for (int i = 0; i < narrative.Length; i++)
        {
            current = narrative.Substring(0, i);
            display.text = current;
            yield return new WaitForSeconds(time);
        }
    }
}
