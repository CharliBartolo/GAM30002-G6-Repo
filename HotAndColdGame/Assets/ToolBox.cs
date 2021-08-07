using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolBox : InteractableBase
{
    public GameObject ToolDisplayObject;
    public GameObject ToolObject;

    private bool triggered;
    public bool destroyOnUse;

    public string itemName;
    private InteractionType interactionType = InteractionType.Use;
    private PlayerInput playerInput;

    private void Start()
    {

    }

    public void OpenTray()
    {
        GetComponent<Animator>().Play("Open");
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef)
    {
        GetComponent<Collider>().enabled = false;
        //playerControls = playerControlsRef;
        ToolDisplayObject.SetActive(false);
        Debug.Log("TOOL REQUESTED");
        triggered = true;
        OpenTray();
        ToolObject.GetComponent<CollectInteractable>().enabled = true;
        Debug.Log("TOOL DELIVERED");

    }
    
    //Runs after interaction is complete
    public override void OnInteractExit()
    {
        //playerControls = null;    
        
    }

    //Runs every frame the interaction continues
    public override void OnInteracting() {}

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
