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

        UpdateLightLists();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!CheckLightListsMatch())
        {
            UpdateLightLists();
        }


        foreach (GameObject activeLight in reflectedLightObjects.Keys)
        {
            if (CheckLightHitsMirror(activeLight.GetComponent<LightTriggers>()))
            {
                UpdateLightPosition(activeLight, reflectedLightObjects[activeLight]);
                reflectedLightObjects[activeLight].SetActive(true);
                reflectedLightObjects[activeLight].GetComponent<Light>().intensity = 20000;
            }
            else
            {
                reflectedLightObjects[activeLight].SetActive(false);
            }
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
                if (!reflectedLightObjects.ContainsKey(lightTrigger.gameObject) && lightTrigger.tag != "ReflectingLight")
                {
                    lightsToUpdate.Add(lightTrigger.gameObject);
                }
            }

            if (lightsToUpdate.Count > 0)
            {
                foreach (GameObject lightToUpdate in lightsToUpdate)
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
                foreach (GameObject lightToUpdate in lightsToUpdate)
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
        reflectedLightClone.tag = "ReflectingLight";
        reflectedLightObjects.Add(lightToClone, reflectedLightClone);
        reflectedLightClone.SetActive(true);
    }

    void RemoveLightClone(GameObject lightToRemoveCloneOf)
    {
        // Delete mirrored light, remove object pair from list
        Destroy(reflectedLightObjects[lightToRemoveCloneOf]);
        reflectedLightObjects.Remove(lightToRemoveCloneOf);
    }

    void UpdateLightPosition(GameObject origLight, GameObject cloneLight)
    {

        // Stops the mirror itself from obstructing the light
        cloneLight.GetComponent<Light>().cullingMask = ~(1 << 6);

        // Reflects cloneLight position to be opposite side of mirror
        Vector3 origLightToMirrorPos = transform.position - origLight.transform.position;
        Vector3 reflectedOrigLightToMirrorPos = Vector3.Reflect(origLightToMirrorPos, transform.forward);
        cloneLight.transform.position = -1 * reflectedOrigLightToMirrorPos + transform.position;

        // Mirror rotation
        //Quaternion origRotation = origLight.transform.rotation;
        Vector3 origFwd = origLight.transform.forward;
        Vector3 mirrored = Vector3.Reflect(origFwd, transform.forward);
        cloneLight.transform.rotation = Quaternion.LookRotation(mirrored, cloneLight.transform.up);
        //Quaternion mirrorNormal = new Quaternion (transform.rotation.x, transform.rotation.y, transform.rotation.z, 0f);
        //cloneLight.transform.rotation = Quaternion.Inverse(origLight.transform.rotation);
    }

    bool CheckLightHitsMirror(LightTriggers lightScript)
    {
        Debug.Log(Vector3.Angle(-lightScript.gameObject.transform.forward, transform.forward));
        if (Vector3.Angle(-lightScript.gameObject.transform.forward, transform.forward) <
            90f) //+ (lightScript.gameObject.GetComponent<Light>().spotAngle / 2))            
        {
            if (lightScript.CheckIfInLightArea(gameObject))
                return true;
            else
                return false;
        }
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

    }
}