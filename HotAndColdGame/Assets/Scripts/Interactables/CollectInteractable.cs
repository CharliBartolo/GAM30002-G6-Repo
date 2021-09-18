using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectInteractable : InteractableBase
{
    public string itemName;
    protected InteractionType interactionType = InteractionType.Use;
    private PlayerInput playerInput;
    public bool destroyOnCollect = true;
    public int int_data;


    public void Start() 
    {
        
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef, float delay = 0)
    {
       switch(itemName)
        {
            case "Raygun":
                StartCoroutine(InteractRaygun(delay));
                break;

            case "Journal":
                StartCoroutine(InteractJournal(delay));
                break;
        }
    }

    IEnumerator InteractRaygun(float delay)
    {
        // wait for animation and stuff
        yield return new WaitForSeconds(delay);

        // do stuff
        GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.SetGunUpgradeState(int_data);
        GameObject.Find("Player").GetComponent<GunFXController>().SetWeaponMods(int_data);

        switch(GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.gunUpgradeState)
        {
            case  RayCastShootComplete.gunUpgrade.None:
                GameObject.Find("Player").GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Neutral);
                Camera.main.GetComponent<AudioSource>().clip = GameObject.Find("Player").GetComponent<PlayerController>().GetComponent<GunFXController>().discoverRaygun;
                Camera.main.GetComponent<AudioSource>().Play();
                break;

            case RayCastShootComplete.gunUpgrade.One:
                GameObject.Find("Player").GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Negative);
                break;

            case RayCastShootComplete.gunUpgrade.Two:
                //GameObject.Find("Player").GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Positive);
                break;
        }

        if (destroyOnCollect)
            Destroy(gameObject);

    }

    IEnumerator InteractJournal(float delay)
    {
        // wait for animation and stuff
        yield return new WaitForSeconds(delay);
        GameObject.Find("UI").GetComponentInChildren<Journal_Reader>().Display_Journal(GetComponent<Journal>().EntryLog, int_data);
        //GameObject.Find("UI").GetComponentInChildren<PauseController>().IsPaused = true;
        // do stuff
        if (destroyOnCollect)
            Destroy(gameObject);
    }


    //Runs after interaction is complete
    public override void OnInteractExit()
    {
        //playerControls = null;      
       /* if(destroyOnCollect)
            Destroy(gameObject);*/
    }

    //Runs every frame the interaction continues
    public override void OnInteracting()
    {        
    }    

    public override InteractionType pInteractionType
    {
        get
        {
            return interactionType;
        }
        set
        {
            interactionType = value;
        } 
    }

    public override PlayerInput pPlayerInput 
    {
        get
        {
            return playerInput;
        }
        set
        {
            playerInput = value;
        } 
    }
}
