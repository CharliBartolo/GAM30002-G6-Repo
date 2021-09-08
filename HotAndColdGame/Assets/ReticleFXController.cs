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

    public override void PerformFX()
    {
        base.PerformFX();

        // check if raycast hit object
        hitTempObject = DetectRangeHit();

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
            return true;
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
                reticle.hotCursor.gameObject.SetActive(false);
                reticle.coldCursor.gameObject.SetActive(false);
                break;
            case ReticleState.Pickup:
                isVisible = true;
                reticle.hotCursor.gameObject.SetActive(false);
                reticle.coldCursor.gameObject.SetActive(false);
                break;
            case ReticleState.Neutral:
               
                reticle.hotCursor.gameObject.SetActive(false);
                reticle.coldCursor.gameObject.SetActive(false);
                break;
            case ReticleState.Negative:

                GameObject[] lightSet_neg = new GameObject[1];
                lightSet_neg[0] = reticle.coldCursor.gameObject;
                negative_emissiive_lights = lightSet_neg;
                reticle.coldCursor.GetComponent<Renderer>().sharedMaterial.color = Crystal_Cold;

                reticle.hotCursor.gameObject.SetActive(false);
                reticle.coldCursor.gameObject.SetActive(true);
                break;
            case ReticleState.Positive:

                GameObject[] lightSet_pos = new GameObject[1];
                lightSet_pos[0] = reticle.hotCursor.gameObject;
                positive_emissiive_lights = lightSet_pos;
                reticle.hotCursor.GetComponent<Renderer>().sharedMaterial.color = Crystal_Hot;

                reticle.hotCursor.gameObject.SetActive(true);
                reticle.coldCursor.gameObject.SetActive(false);
                break;
            case ReticleState.Machine:
                reticle.hotCursor.gameObject.SetActive(false);
                reticle.coldCursor.gameObject.SetActive(true);
                break;
        }
    }

    public void UpdateReticleSize()
    {
        if (reticle.Gun.cold)
        {
            float size = (Camera.main.transform.position - reticle.coldCursor.transform.position).magnitude;
            Vector3 newSize = new Vector3(size, size, size) * reticleSize;
            reticle.coldCursor.transform.localScale = newSize;
        }
        else
        {
            float size = (Camera.main.transform.position - reticle.hotCursor.transform.position).magnitude;
            Vector3 newSize = new Vector3(size, size, size) * reticleSize;
            reticle.hotCursor.transform.localScale = newSize;
        }
    }
}
