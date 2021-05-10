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

    private GameObject cursorInstance;
    private GameObject cursorInstance2;

    void Start () {
        cursorInstance = Instantiate(coldPre); //Draw Reticle
        cursorInstance2 = Instantiate(heatPre); //Draw Reticle
 
    }
	
	void LateUpdate () {
        UpdateCursor(); //Update Smoothly
        cold = Gun.cold; //Check Gun State (Bad, Change this)
        if (cold)
        {
            cursorInstance2.gameObject.SetActive(false);
            cursorInstance.gameObject.SetActive(true);
        }
        else
        {
            cursorInstance2.gameObject.SetActive(true);
            cursorInstance.gameObject.SetActive(false);
        }
    }

    private void UpdateCursor()
    {
        Ray ray = new Ray(Cam.transform.position, Cam.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            cursorInstance.transform.position = hit.point;
            cursorInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            cursorInstance2.transform.position = hit.point;
            cursorInstance2.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            cursorInstance.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            cursorInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);

            cursorInstance2.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            cursorInstance2.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);
        }
    }
}
