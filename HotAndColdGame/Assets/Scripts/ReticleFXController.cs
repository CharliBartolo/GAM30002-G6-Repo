using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleFXController : ToolboxFXController
{
    [Header("Components")]
    public Reticle reticle;
    [Header("Reticle Properties")]

    public bool isHidden = false;
    public float reticleSize = 0.25f;
    public float reticleOpacity = 0.5f;

    // raycast components
    Vector3 rayOrigin = Vector3.zero;
    Camera cam = null;

    // range properties
    public float range_raygun;
    public float range_interact;

    // reticle display properties
    public float size_unequipped = 0.09f;
    public float size_pickup = 0.09f;
    public float size_negative = 0.05f;
    public float size_positive = 0.05f;
    public float size_neutral = 0.05f;
    

    public GameObject objHit{ get; set;}

    public enum ReticleState { Unequipped, Pickup, Neutral, Negative, Positive, Machine, }

    public ReticleState state = ReticleState.Unequipped;
    ReticleState prevState = ReticleState.Unequipped;

    // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();

        cam = GetComponentInChildren<Camera>();
       
        range_raygun = GetComponentInChildren<RayCastShootComplete>().weaponRange;
        range_interact = GetComponentInChildren<PlayerController>().interactRange;
       
        

        ChangeState(ReticleState.Unequipped);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHidden)
            Hide();
        else
            Show();
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
       
        ResponsiveReticle();
       
        

    }

    public void ResponsiveReticle()
    {
        DetectObjectAhead();
        //Debug.Log("HIT OBJECT: " + objHit);

        bool hitCrystal = false;
        bool hitCollectable = false;


        if (objHit != null)
        {
            // check for temp state based object
            if (objHit.GetComponentInParent<CrystalBehaviour>() != null)
            {
                Debug.Log("HIT CRYSTAL OBJECT: " + objHit);
                if (!hitCrystal)
                    hitCrystal = true;

            }
            // check for collectable object
            else if (objHit.GetComponent<CollectInteractable>() != null)
            {
                Debug.Log("HIT COLLECTABLE OBJECT: " + objHit);

                // check if within interact range
                if (Vector3.Distance(objHit.transform.position, cam.transform.position) < range_interact)
                {
                    if (state != ReticleState.Pickup)
                    {
                        prevState = state;
                        ChangeState(ReticleState.Pickup);
                    }

                    if (!hitCollectable)
                        hitCollectable = true;
                }
                else
                {
                    if (hitCollectable)
                        hitCollectable = false;
                }
               
              
            }
            else
            {
                hitCrystal = false;
                hitCollectable = false;

                if (state != prevState)
                {
                    if(state == ReticleState.Negative)
                    {
                        prevState = ReticleState.Negative;
                    }
                    else if (state == ReticleState.Positive)
                    {
                        prevState = ReticleState.Positive;
                    }

                    ChangeState(prevState);
                }
                    
                //ChangeState(ReticleState.Unequipped);
                //UpdateReticalState();
            }
           

        }
        // return to prev reticle state
        else
        {
           
        }
    }

    public void DetectObjectAhead()
    {
        RaycastHit hit;
        rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        bool hitSomething = Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 100);

        if(hitSomething)
        {
            objHit = hit.collider.gameObject;
        }
        else
        {
            objHit = null;
        }
    }

   /* public bool DetectRangeHit()
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
    }*/

    public void ChangeState(ReticleState newState)
    {
        //prevState = state;
        //Debug.Log("PREVSTATE: " +  prevState);
        state = newState;
        UpdateReticalState();
    }
    
    public void UpdateReticalState()
    {
        switch(state)
        {
            case ReticleState.Unequipped:
                //isHidden = true;
                Color colUnequipped = Color.white;
                colUnequipped.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colUnequipped;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.neutralCursor;
                reticleSize = size_unequipped;
                /* reticle.hotCursor.gameObject.SetActive(false);
                 reticle.coldCursor.gameObject.SetActive(false);*/
                break;
            case ReticleState.Pickup:
                //isHidden = true;
                Color colPickup = Color.white;
                colPickup.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colPickup;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.hand;
                reticleSize = size_pickup;
                break;
            case ReticleState.Neutral:
                Color colNeutral = Color.white;
                colNeutral.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colNeutral;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.neutralCursor;
                reticleSize = size_unequipped;
                break;
            case ReticleState.Negative:
                Color colCold = Crystal_Cold;
                colCold.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colCold;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.coldCursor;
                reticleSize = size_negative;
                break;
            case ReticleState.Positive:
                Color colHot = Crystal_Hot;
                colHot.a = reticleOpacity;
                reticle.Cursor.GetComponent<SpriteRenderer>().color = colHot;
                reticle.Cursor.GetComponent<SpriteRenderer>().sprite = reticle.hotCursor;
                reticleSize = size_positive;
                break;
            case ReticleState.Machine:
                reticleSize = reticleOpacity;
                size_unequipped = size_neutral;
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
