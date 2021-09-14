using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleFXController : ToolboxFXController
{
    [Header("Components")]
    public Reticle reticle;
    [Header("Reticle Properties")]

    public bool isVisible = false;
    public float reticleSize = 0.25f;
    public float reticleOpacity = 0.5f;

    public bool hitTempObject = false;

    public GameObject objHit{ get; set;}

    public enum ReticleState { Unequipped, Pickup, Neutral, Negative, Positive, Machine, }

    public ReticleState state = ReticleState.Unequipped;

    // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();

        ChangeState(ReticleState.Unequipped);
    }

    // Update is called once per frame
    void Update()
    {
        PerformFX();
    }

    public void Show()
    {
        reticle.Cursor.SetActive(true);
        //reticle.hotCursor.SetActive(true);
    }

    public void Hide()
    {
        reticle.Cursor.SetActive(false);
        //reticle.hotCursor.SetActive(false);
    }

    public override void PerformFX()
    {
        base.PerformFX();

        // check if looking at hit object
       /* if (DetectRangeHit())
        {
            Show();
        }
        else
        {
            Hide();
        }*/

        // update reticle size
        UpdateReticleSize();

        ReticleBehaviour();

    }
    
    public void ReticleBehaviour()
    {

        if (hitTempObject)
        {
         

        }
        else
        {

        }
    }

    public bool DetectRangeHit()
    {
        if(objHit != null)
        {
            Camera cam = GetComponentInChildren<Camera>();
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, GetComponent<RayCastShootComplete>().weaponRange))
            {
                return true;
               
            }
                
        }
        return false;
    }

    public void ChangeState(ReticleState newState)
    {
        state = newState;
        UpdateReticalState();
    }
    
    public void UpdateReticalState()
    {
        switch(state)
        {
            case ReticleState.Unequipped:
                isVisible = false;
                Color colUnequipped = Color.white;
                colUnequipped.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colUnequipped;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.neutralCursor;
                reticleSize = 0.085f;
                /* reticle.hotCursor.gameObject.SetActive(false);
                 reticle.coldCursor.gameObject.SetActive(false);*/
                break;
            case ReticleState.Pickup:
                isVisible = true;
                Color colPickup = Color.white;
                colPickup.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colPickup;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.hand;
                reticleSize = 2f;
                break;
            case ReticleState.Neutral:
                Color colNeutral = Color.white;
                colNeutral.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colNeutral;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.neutralCursor;
                reticleSize = 0.085f;
                break;
            case ReticleState.Negative:
                Color colCold = Crystal_Cold;
                colCold.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colCold;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.coldCursor;
                reticleSize = 0.05f;
                break;
            case ReticleState.Positive:
                Color colHot = Crystal_Hot;
                colHot.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colHot;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.hotCursor;
                reticleSize = 0.05f;
                break;
            case ReticleState.Machine:
                reticleSize = reticleOpacity;
                /* reticle.hotCursor.gameObject.SetActive(false);
                 reticle.coldCursor.gameObject.SetActive(true);*/
                break;
        }
    }

    public void UpdateReticleSize()
    {
        if (reticle.Gun.cold)
        {
            float size = (Camera.main.transform.position - reticle.Cursor.transform.position).magnitude;
            Vector3 newSize = new Vector3(size, size, size) * reticleSize;
            reticle.Cursor.transform.localScale = newSize;
        }
        else
        {
            float size = (Camera.main.transform.position - reticle.Cursor.transform.position).magnitude;
            Vector3 newSize = new Vector3(size, size, size) * reticleSize;
            reticle.Cursor.transform.localScale = newSize;
        }
    }
}
