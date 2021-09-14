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

    //public GameObject coldCursor;
    //public GameObject hotCursor;
    public Sprite coldCursor;
    public Sprite hotCursor;
    public Sprite neutralCursor;
    public Sprite hand;
    public GameObject Cursor;

 
    void Start() 
    {
        //coldCursor = Instantiate(coldPre); //Draw Reticle
        //hotCursor = Instantiate(heatPre); //Draw Reticle
    }
	
	void LateUpdate() {
        if (GetComponent<RayCastShootComplete>() != null)
            UpdateCursorPosition(); //Update Smoothly       
       /* if (Gun.cold)
        {
            //hotCursor.gameObject.SetActive(false);
            //coldCursor.gameObject.SetActive(true);
        }
        else
        {
            //hotCursor.gameObject.SetActive(true);
            //coldCursor.gameObject.SetActive(false);
        }*/
    }

    private void UpdateCursorPosition()
    {
      
        Ray ray = new Ray(Cam.transform.position, Cam.transform.rotation * Vector3.forward);
        RaycastHit hit;
        float distance = GetComponent<RayCastShootComplete>().weaponRange;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))

        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.Log("HIT AHEAD");
            GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Pickup);
            
            Cursor.transform.position = hit.point + hit.normal * 0.05f;
            Cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

            /*if(hit.collider.gameObject.GetComponent<CollectInteractable>() != null)
            {
                GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Pickup);
            }*/

            //coldCursor.transform.position = hit.point;
            /*coldCursor.transform.position = hit.point + hit.normal * 0.125f;
            coldCursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);*/

            //coldCursor.transform.rotation = hit.rotation;
            //hotCursor.transform.position = hit.point;
            /*hotCursor.transform.position = hit.point + hit.normal * 0.125f;
            hotCursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);*/
            //hotCursor.transform.rotation = hit.rotation;
        }
        else
        {
            Debug.Log("NO HIT AHEAD");
            GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Neutral);
            Cursor.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            Cursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);
            /*coldCursor.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            coldCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);

            hotCursor.transform.position = ray.origin + ray.direction.normalized * maxDistance;
            hotCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);*/
        }
    }
}
