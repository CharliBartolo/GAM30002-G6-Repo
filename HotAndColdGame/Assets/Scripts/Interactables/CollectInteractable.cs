using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectInteractable : InteractableBase
{
    public string itemName;
    private InteractionType interactionType = InteractionType.Use;
    private PlayerInput playerInput;
    public bool destroyOnCollect = true;
    public int int_data;

    private void Start() 
    {
        
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef)
    {        

       switch(itemName)
        {
            case "Raygun":
                GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.SetGunUpgradeState(int_data);
                GameObject.Find("Player").GetComponent<GunFXController>().SetWeaponMods(int_data);
                break;

            case "Journal":
                GameObject.Find("UI").GetComponentInChildren<Journal_Reader>().Display_Journal(GetComponent<Journal>().EntryLog, int_data);
                //GameObject.Find("UI").GetComponentInChildren<PauseController>().IsPaused = true;
                break;
        }
    }

    //Runs after interaction is complete
    public override void OnInteractExit()
    {
        //playerControls = null;      
        if(destroyOnCollect)
            Destroy(gameObject);
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
