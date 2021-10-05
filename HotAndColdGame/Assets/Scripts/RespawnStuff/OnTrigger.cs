using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    public bool Enter = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger script");
        Enter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Enter = false;
    }
}
