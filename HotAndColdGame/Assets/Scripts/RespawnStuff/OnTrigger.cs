using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
/// <summary>
/// This class acts as a supplemental attachment to loading
/// Trigger scripts.
/// Last edit: Added checks to ensure Player is object triggering loads.
/// By: Charli - 6/10/21
/// </summary>
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
public class OnTrigger : MonoBehaviour
{
    public bool Enter = false;

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        //Debug.Log("trigger script");
        if (other == GameMaster.instance.playerRef.gameObject.GetComponent<Collider>())
        {
            Enter = true;
        }
        
=======
        Debug.Log("trigger script");
        Enter = true;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }

    private void OnTriggerExit(Collider other)
    {
<<<<<<< HEAD
        if (other == GameMaster.instance.playerRef.gameObject.GetComponent<Collider>())
        {
            Enter = false;
        }
=======
        Enter = false;
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }
}
