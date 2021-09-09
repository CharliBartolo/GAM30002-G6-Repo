using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp2 : InteractableBase
{
    public Transform theDest;// for player to see the object they're holding
    
    private InteractionType interactionType = InteractionType.Carry;
    private PlayerInput playerInput;

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef, float delay = 0)
    {
        playerInput = playerInputRef;

        Debug.Log("Mouse down triggered");
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false; //allows picking up object
    }

    //Runs when interaction ceases
    public override void OnInteractExit()
    {
        playerInput = null;

        Debug.Log("Mouse up triggered");
        this.transform.parent = null;//drop item in current position
        GetComponent<Rigidbody>().useGravity = true;//item falls to the ground
        GetComponent<BoxCollider>().enabled = true;
    }

    //Runs every frame the interaction continues
    public override void OnInteracting()
    {       
        this.transform.position = theDest.position;
        this.transform.parent = theDest.transform; //so player can see object when being held
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
