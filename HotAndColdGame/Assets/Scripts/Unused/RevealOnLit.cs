using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealOnLit : MonoBehaviour
{
    public float startingTimeTillLightTransitions = 0.1f;

    private LightDetection lightDetection;
    private MeshRenderer meshRenderer;
    private float timeTillLightTransitions;    
    private bool prevLitState;

    // Start is called before the first frame update
    void Start()
    {
        lightDetection = GetComponent<LightDetection>();
        meshRenderer = GetComponent<MeshRenderer>();
        timeTillLightTransitions = startingTimeTillLightTransitions;
        SetObjectVisible(lightDetection.isLit);
        prevLitState = lightDetection.isLit;       
    }

    // Update is called once per frame
    void Update()
    {
        // If lighting state changes (i.e Light into dark or dark into light), wait x time, then swap if state is consistent
        if (prevLitState != lightDetection.isLit)
        {
            // If enough time has passed and lighting is the same, swap state.
            if (timeTillLightTransitions <= 0f)
            {
                SetObjectVisible(!prevLitState);
                prevLitState = !prevLitState;
            }
            else
            {
                timeTillLightTransitions -= Time.deltaTime;
            }
        }
        else
        {
            timeTillLightTransitions = startingTimeTillLightTransitions;
        }        
    }

    void SetObjectVisible(bool isVisible)
    {
        meshRenderer.enabled = isVisible;
    }
}
