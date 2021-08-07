using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class FindDuplicateColliders : MonoBehaviour
{
    //public List<GameObject> objectsInScene = new List<GameObject>();
    public List<GameObject> duplicateColliderObjects = new List<GameObject>();
    public List<GameObject> multipleMaterialObjects = new List<GameObject>();

    private void OnEnable()
    {
        SortDuplicateColliders(GetAllObjectsInScene());
        SortMultipleMaterialObjectsColliders(GetAllObjectsInScene());
    }

    void SortMultipleMaterialObjectsColliders(GameObject[] objs)
    {
        foreach (var item in objs)
        {
            if (item.GetComponent<Renderer>() != null )
            {
                if (item.GetComponent<Renderer>().sharedMaterials != null && item.GetComponent<Renderer>().sharedMaterials.Length > 1)
                {
                    multipleMaterialObjects.Add(item);
                }
            }
        }
    }
    void SortDuplicateColliders(GameObject[] objs)
    {
        foreach (var item in objs)
        {
            if (item.GetComponent<Collider>() != null || item.GetComponents<Collider>() != null)
            {
                if (item.GetComponents<Collider>().Length > 1)
                {
                    duplicateColliderObjects.Add(item);
                }
            }
        }
    }
    public GameObject[] GetAllObjectsInScene()
    {
        return Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
    }
}
