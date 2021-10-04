using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChecker : MonoBehaviour
{
    // Update is called once per frame
    public string GetCurrentSurface()
    {
        string materialName = "";
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5))
        {
            if (hit.collider != null && hit.collider.CompareTag("Surface"))
            {
                materialName = hit.collider.GetComponent<Renderer>().material.name;
                print(materialName);
            }
        }
        return materialName;
    }
}
