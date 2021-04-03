using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMirror : MonoBehaviour
{
    // Need list of lights hitting this object
    // Also need to check they're hitting the right side (use normals)
    // if Light hits correct side, copy light object, adjust angle and length, reflect its placement
    // Use LightDetection's list of lights

    LightDetection lightDetection;
    public Dictionary<GameObject, GameObject> reflectedLightObjects;
    

    // Start is called before the first frame update
    void Start()
    {
        reflectedLightObjects = new Dictionary<GameObject, GameObject>();
        lightDetection = GetComponent<LightDetection>();

        foreach (LightTriggers lightSource in lightDetection.activeLights)
        {   
            AddLightClone(lightSource.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckLightListsMatch())
        {
            UpdateLightLists();
        }        
    }

    // True if lists match, false if they do not
    bool CheckLightListsMatch()
    {
        //Debug.Log(reflectedLightObjects.Count);
        if (lightDetection.activeLights.Count != reflectedLightObjects.Count)
        {
            return false;
        }
        return true;
    }

    void UpdateLightLists()
    {
        List<GameObject> lightsToUpdate = new List<GameObject>();

        if (lightDetection.activeLights.Count > reflectedLightObjects.Count)
            {
                // LightDetection has more, so we have to add more lights to dictionary
                foreach (LightTriggers lightTrigger in lightDetection.activeLights)
                {
                    if (!reflectedLightObjects.ContainsKey(lightTrigger.gameObject))
                    {
                        lightsToUpdate.Add(lightTrigger.gameObject);
                    }
                }

                if (lightsToUpdate.Count > 0)
                {
                    foreach(GameObject lightToUpdate in lightsToUpdate)
                    {
                        AddLightClone(lightToUpdate);
                    }
                }
            }
            else 
            {
                // LightDetection has less lights, so we have to remove lights from dictionary
                foreach (KeyValuePair<GameObject, GameObject> reflectedLight in reflectedLightObjects)
                {
                    // If lightDetection doesn't have an object found in reflectedLight, remove it
                    if (!lightDetection.activeLights.Contains(reflectedLight.Key.GetComponent<LightTriggers>()))
                    {
                        lightsToUpdate.Add(reflectedLight.Key);
                    }                    
                }

                if (lightsToUpdate.Count > 0)
                {
                    foreach(GameObject lightToUpdate in lightsToUpdate)
                    {
                        RemoveLightClone(lightToUpdate);
                    }
                }                
            }
    }

    void AddLightClone(GameObject lightToClone)
    {
        GameObject reflectedLightClone = Instantiate(lightToClone);
        reflectedLightClone.SetActive(false);  
        reflectedLightObjects.Add(lightToClone, reflectedLightClone);
    }

    void RemoveLightClone(GameObject lightToRemoveCloneOf)
    {
        // Delete mirrored light, remove object pair from list
        Destroy(reflectedLightObjects[lightToRemoveCloneOf]);
        reflectedLightObjects.Remove(lightToRemoveCloneOf);
    }
}
