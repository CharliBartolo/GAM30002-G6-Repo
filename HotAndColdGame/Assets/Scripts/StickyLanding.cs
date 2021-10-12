 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyLanding : MonoBehaviour
{
    private Transform parent;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* private void OnCollisionEnter(Collision collision)
     {
         if(collision.gameObject.GetComponent<Rigidbody>().velocity.y <=0)
         {
             parent = collision.gameObject.transform.parent;
             collision.gameObject.transform.parent = this.transform;
         }
     }*/

    private void FixedUpdate()
    {
        UpdateParents();


    }

    void UpdateParents()
    {
        if (this.player != null)
        {
            if (this.parent != null)
            {
                this.player.transform.parent = parent;
            }
            else
            {
                this.player.transform.parent = null;
            }
            this.player = null;
        }
    }

    void CheckPlayer()
    {

    }


    void OnCollisionStay(Collision collisionInfo)
    {
        if (player == null)
        {
            if (collisionInfo.gameObject == GameMaster.instance.playerRef)
            {
                Debug.Log("PLAYER ON PLATFORM");
                parent = collisionInfo.gameObject.transform.parent;
                collisionInfo.gameObject.transform.parent = this.transform;
                this.player = collisionInfo.gameObject;
            }
        }
    }

   /* private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == GameMaster.instance.playerRef)
        {
            Debug.Log("PLAYER EXIT PLATFORM");

            if (parent != null)
            {
                collision.gameObject.transform.parent = parent;
            }
            else
            {
                collision.gameObject.transform.parent = null;
            }
        }
    }*/
}
