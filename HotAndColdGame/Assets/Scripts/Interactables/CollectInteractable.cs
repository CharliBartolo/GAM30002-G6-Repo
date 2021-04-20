using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectInteractable : InteractableBase
{
    private InteractionType interactionType = InteractionType.Use;
    private PlayerFPControls playerControls;

    private void Start() 
    {
        
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerFPControls playerControlsRef)
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

    public override PlayerFPControls pPlayerFPControls 
    {
        get
        {
            return playerControls;
        }
        set
        {
            playerControls = value;
        } 
    }
}
