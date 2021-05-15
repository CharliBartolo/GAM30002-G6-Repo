using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyLanding : MonoBehaviour
{

    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>().velocity.y <=0)
        {
            parent = collision.gameObject.transform.parent;
            collision.gameObject.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (parent != null)
        {
            collision.gameObject.transform.parent = parent;
        }
        else
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
