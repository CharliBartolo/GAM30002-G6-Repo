using UnityEngine;
using System.Collections;
using System;

public class Reticle : MonoBehaviour {
    public Camera Cam;
    public GameObject coldPre;
    public GameObject heatPre;
    public float maxDistance = 30;
    public RayCastShootComplete Gun;
    public bool cold = true;

    public GameObject coldCursor;
    public GameObject hotCursor;

    

    void Start() 
    {
        coldCursor = Instantiate(coldPre); //Draw Reticle
        hotCursor = Instantiate(heatPre); //Draw Reticle
    }
	
	void LateUpdate() {
        UpdateCursorPosition(); //Update Smoothly       
       /* if (Gun.cold)
        {
            hotCursor.gameObject.SetActive(false);
            coldCursor.gameObject.SetActive(true);
        }
        else
        {
            hotCursor.gameObject.SetActive(true);
            coldCursor.gameObject.SetActive(false);
        }*/
    }

    private void UpdateCursorPosition()
    {
      

        Ray ray = new Ray(Cam.transform.position, Cam.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            coldCursor.transform.position = hit.point + hit.normal * 0.05f;
            coldCursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            //coldCursor.transform.rotation = hit.rotation;

            hotCursor.transform.position = hit.point + hit.normal * 0.05f;
            hotCursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            //hotCursor.transform.rotation = hit.rotation;
        }
        else
        {
            coldCursor.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            coldCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);

            hotCursor.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            hotCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);
        }
    }
}
