using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectInteractable : InteractableBase
{
    public string itemName;
    private InteractionType interactionType = InteractionType.Use;
    private PlayerInput playerInput;

    private void Start() 
    {
        
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef)
    {        
        
        //playerControls = playerControlsRef;
    }

    //Runs after interaction is complete
    public override void OnInteractExit()
    {
        //playerControls = null;        
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
