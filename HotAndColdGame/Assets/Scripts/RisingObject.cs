using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RisingObject : MonoBehaviour
{
    public GameObject _object;
    public bool isEnabled;
    //public float speed;
    public float timeToRise;
    public Vector3 maxHeight;

    public bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered)
        {
            Perform();
        }
    }

    public void Perform()
    {
        _object.transform.DOMoveY(maxHeight.y, timeToRise);
    }


    public void TriggerStart()
    {
        if (!triggered)
        {
            triggered = true;
            GetComponent<AudioSource>().Play();
        }
           
    }
}
