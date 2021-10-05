using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRaygunFXController : ToolboxFXController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Crystal_Hot /= colourIntensity;
        Crystal_Cold /= colourIntensity;
        Crystal_Neutral /= colourIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        base.PerformFX();
    }
}
