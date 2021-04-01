using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOPunchPosition(transform.forward, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

       

    }

    void PrintDone()
    {
        Debug.Log("DONE");
    }
}
