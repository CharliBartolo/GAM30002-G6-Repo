using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHatch : StateTriggered
{
    public enum DoorState { Locked, Open1, Open2, Close1, Close2 }
    public DoorState state;

    private bool stateEntered;

    private Animator Anim;
    private bool isAnimationComplete;

    public bool delay;
    private bool delayTimer;
    public bool stateChanged;

    public GameObject[] EmmisiveLights_Positive;
    public GameObject[] EmmisiveLights_Negative;

    private Material emissiveMaterial;
    public float emissionValue;

    protected Color Crystal_Neutral;
    protected Color Crystal_Hot;
    protected Color Crystal_Cold;

    //public LineRenderer Lightning;

    //public TemperatureStateBase machine;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Crystal_Hot = GameMaster.instance.colourPallete.Positive;
        Crystal_Cold = GameMaster.instance.colourPallete.Negative;
        Crystal_Neutral = GameMaster.instance.colourPallete.Neutral;

        stateChanged = false;
        state = DoorState.Locked;
        Anim = GetComponentInChildren<Animator>();
        emissiveMaterial = GameMaster.instance.colourPallete.materials.EmissiveLights;
        InitialiseLights();

    }

    // Update is called once per frame
    void Update()
    {
        if(Trigger != null && AnimationComplete())
            ListenForTrigger();
    }

    public override void ListenForTrigger()
    {
        isAnimationComplete = AnimationComplete();

        switch (Trigger.CurrentTempState)
        {
            case ITemperature.tempState.Cold:
                stateChanged = true;
                
                if (state == DoorState.Locked)
                {
                    
                    if (isAnimationComplete)
                    {
                        Anim.Play("Open1");
                        state = DoorState.Open1;
                        //ActivateLight(-1);
                        DeactivateLight(-1);
                        stateChanged = false;
                        Debug.Log("STATE CHANGED");
                    }
                }else if (state == DoorState.Open1)
                {
                    if (isAnimationComplete)
                    {
                        Anim.Play("Close1");
                        state = DoorState.Locked;
                        ActivateLight(-1);
                        ActivateLight(1);
                        //DeactivateLight(-1);
                        //DeactivateLight(1);
                        stateChanged = false;
                    }
                
                    break;
                }
              
                break;

            case ITemperature.tempState.Hot:

                stateChanged = true;

                if (state == DoorState.Open1)
                {
                    if (isAnimationComplete)
                    {
                        state = DoorState.Open2;
                        Anim.Play("Open2");
                        //ActivateLight(-1);
                        DeactivateLight(1);
                        //DeactivateLight(-1);
                        stateChanged = false;
                    }
                }else if (state == DoorState.Open2)
                {
                    if (isAnimationComplete)
                    {
                        state = DoorState.Open1;
                        Anim.Play("Close2");
                        
                        //ActivateLight(-1);
                        ActivateLight(1);
                        //DeactivateLight(1);
                        stateChanged = false;
                    }
                    break;
                }
       
                break;
        }
    }


    public void InitialiseLights()
    {
        foreach (var item in EmmisiveLights_Positive)
        {
            Renderer[] rs = item.GetComponentsInChildren<Renderer>();

            foreach (var r in rs)
            {
                if (r != null)
                {
                    r.sharedMaterial = new Material(emissiveMaterial);
                }
            }
        }

        foreach (var item in EmmisiveLights_Negative)
        {
            Renderer[] rs = item.GetComponentsInChildren<Renderer>();

            foreach (var r in rs)
            {
                if (r != null)
                {
                    r.sharedMaterial = new Material(emissiveMaterial);
                }
            }
        }

        ActivateLight(-1);
        ActivateLight(1);
    }
    public void ActivateLight(int light)
    {
        if (light == 1)
        {
            foreach (var item in EmmisiveLights_Positive)
            {
                Renderer[] rs = item.GetComponentsInChildren<Renderer>();

                foreach (var r in rs)
                {
                    if (r != null)
                    {
                        //r.sharedMaterial = new Material(emissiveMaterial);
                        r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Hot);
                    }
                }
                //item.SetActive(true);
            }
        }
        else if (light == -1)
        {
            foreach (var item in EmmisiveLights_Negative)
            {
                Renderer[] rs = item.GetComponentsInChildren<Renderer>();

                foreach (var r in rs)
                {
                    if (r != null)
                    {
                        //r.sharedMaterial = new Material(emissiveMaterial);
                        r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Cold);
                    }
                }
                //item.SetActive(true);
            }
        }
    }
    public void DeactivateLight(int light)
    {
        if (light == 1)
        {
            foreach (var item in EmmisiveLights_Positive)
            {
                Renderer[] rs = item.GetComponentsInChildren<Renderer>();


                foreach (var r in rs)
                {
                    if (r != null)
                    {
                        //r.sharedMaterial = new Material(emissiveMaterial);
                        r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral);
                    }
                }
                //item.SetActive(false);
            }
        }
        else if (light == -1)
        {
            foreach (var item in EmmisiveLights_Negative)
            {
                Renderer[] rs = item.GetComponentsInChildren<Renderer>();


                foreach (var r in rs)
                {
                    if (r != null)
                    {
                        //r.sharedMaterial = new Material(emissiveMaterial);
                        r.sharedMaterial.SetColor("_EmissiveColor", Crystal_Neutral);
                    }
                }
                //item.SetActive(false);
            }
        }
    }

    /* public void SetLights(GameObject[] lights, int state)
     {
         switch(state)
         {
             case -1:
                 foreach (var item in lights)
                 {
                     Renderer[] rs = item.GetComponentsInChildren<Renderer>();
                     foreach (var r in rs)
                     {
                         if (r != null)
                         {
                             //r.sharedMaterial = emissiveMaterial;
                             r.sharedMaterial.SetColor("_EmissiveColor", GameMaster.instance.colourPallete.Negative);
                         }
                     }

                 }
             break;

             case 0:
                 foreach (var item in lights)
                 {
                     Renderer[] rs = item.GetComponentsInChildren<Renderer>();

                     if (rs != null)
                     {
                         foreach (var r in rs)
                         {
                             //r.sharedMaterial = emissiveMaterial;
                             r.sharedMaterial.SetColor("_EmissiveColor", GameMaster.instance.colourPallete.Neutral);
                         }
                     }
                 }
                 break;

             case 1:
                 foreach (var item in lights)
                 {
                     Renderer[] rs = item.GetComponentsInChildren<Renderer>();

                     if (rs != null)
                     {
                         foreach (var r in rs)
                         {
                             //r.sharedMaterial = emissiveMaterial;
                             r.sharedMaterial.SetColor("_EmissiveColor", GameMaster.instance.colourPallete.Positive);
                         }
                     }
                 }
                 break;
         }


         foreach (var item in lights)
         {
             Renderer[] rs = item.GetComponentsInChildren<Renderer>();

             if (rs != null)
             {
                 foreach (var r in rs)
                 {
                     //r.sharedMaterial = emissiveMaterial;
                     r.sharedMaterial.SetColor("_EmissiveColor", GameMaster.instance.colourPallete.Neutral);
                 }
             }
         }
     }
 */

    public virtual bool AnimationComplete()
    {
        return (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !Anim.IsInTransition(0));
    }

}
