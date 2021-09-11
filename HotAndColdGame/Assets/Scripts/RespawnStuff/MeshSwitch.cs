using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSwitch : MonoBehaviour
{
    private Renderer mesh; //not sure if need as array since scene is a prefab or if it needs to be in the start function
    //public GameObject myPrefab;
    //GameObject myPrefabInstance;

    private void Start()
    {
        mesh = GetComponent<Renderer>();
        mesh.enabled = true;
        //myPrefabInstance = Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }

    //toggle visibility
    private void Update()
    {
        if (Input.GetKeyDown("g")) 
        {
            //render and unrender objects 
            mesh.enabled = !mesh.enabled;        
            /*for (int i = 0; i < mesh.length; i++) 
            {
                //render and unrender objects 
                mesh[i].enabled = !mesh.enabled;
            }*/
            //myPrefabInstance.GetComponent<MeshRenderer>().enabled = false;
            //myPrefabInstance.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
