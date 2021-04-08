using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    public bool isLit;
    public List<LightTriggers> activeLights;    

    //public event EnteredLightTriggerHandler PlayerEnteredLight;

    // Start is called before the first frame update
    void Start()
    {
        activeLights = new List<LightTriggers>();        
    }

    // Update is called once per frame
    void Update()
    {
        isLit = CheckIfLit();
    }

    bool CheckIfLit()
    {          
        foreach (LightTriggers light in activeLights)
        {
            if (light.CheckIfInLightArea(this.gameObject))
            {
                return true;
            }
        }

        return false;
    }

    public void AddLight(LightTriggers light)
    {
        activeLights.Add(light);
    }

    public void RemoveLight(LightTriggers light)
    {
        activeLights.Remove(light);
    }
<<<<<<< HEAD

    void OnTriggerEnter(Collider other)
    {
        //isPlayerInLight = true;

        //Debug.Log("Object has entered light area");
        if (other.GetComponent<LightTriggers>() != null && !activeLights.Contains(other.GetComponent<LightTriggers>()))
        {
            AddLight(other.GetComponent<LightTriggers>());
        }    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LightTriggers>() != null && activeLights.Contains(other.GetComponent<LightTriggers>()))
        {
            RemoveLight(other.GetComponent<LightTriggers>());
        }  
    }
=======
>>>>>>> parent of 3027a01d (Updated LightDetection to have two separate modes: complex and simple detection. Complex uses more rays)
    
     void OnGUI()
     {
         GUILayout.BeginArea(new Rect(10f, 10f, Screen.width, Screen.height)); 

         GUILayout.Label("Player in light: " + isLit);
 
         GUILayout.EndArea();
     }
}
