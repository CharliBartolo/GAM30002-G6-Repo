using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class FindDuplicateColliders : MonoBehaviour
{
    public List<GameObject> objectsInScene = new List<GameObject>();
    public void  GetAllObjectsInScene()
    {
       

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags != HideFlags.None)
                continue;


            objectsInScene.Add(go);
        }
    }
}
