using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as a supplemental attachment to loading
/// Trigger scripts.
/// Last edit: Added checks to ensure Player is object triggering loads.
/// By: Charli - 6/10/21
/// </summary>
public class OnTrigger : MonoBehaviour
{
    public bool Enter = false;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger script");
        if (other == GameMaster.instance.playerRef.gameObject.GetComponent<Collider>())
        {
            Enter = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == GameMaster.instance.playerRef.gameObject.GetComponent<Collider>())
        {
            Enter = false;
        }
    }
}
