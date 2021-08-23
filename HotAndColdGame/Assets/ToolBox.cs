using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolBox : InteractableBase
{
    public GameObject ToolDisplayObject;
    public GameObject ToolObject;

    public float upgradeTime;

    private bool triggered;
    //public bool destroyOnUse;

    //public string itemName;
    private InteractionType interactionType = InteractionType.Use;
    private PlayerInput playerInput;
    //public int gunUpgradeLevel = 0;

    private void Start()
    {
        ToolDisplayObject.SetActive(true);
        ToolObject.SetActive(false);
        OpenTray();
    }

    public void OpenTray()
    {
        GetComponent<Animator>().speed = 1;
        GetComponent<Animator>().Play("Open");
    }

    public IEnumerator RunUpgradeSequence()
    {
        GetComponent<Animator>().speed = 2;
        GetComponent<Animator>().Play("Close");
        

        yield return new WaitForSeconds(upgradeTime);

        GetComponent<Animator>().speed = 2;
        GetComponent<Animator>().Play("Open");
        ToolDisplayObject.SetActive(false);
        ToolObject.SetActive(true);
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef)
    {
        RayCastShootComplete.gunUpgrade currentGunLevel = GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.gunUpgradeState;

        if((int)currentGunLevel < 2 && GameObject.Find("Player").GetComponent<GunFXController>().equipped)
        {
            GetComponent<Collider>().enabled = false;
            //playerControls = playerControlsRef;
            
            Debug.Log("TOOL REQUESTED");
            triggered = true;
            //GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.SetGunUpgradeState((int)currentGunLevel + 1);
            GameObject.Find("Player").GetComponent<PlayerController>().isGunEnabled = false;
            GameObject.Find("Player").GetComponent<GunFXController>().UnEquipTool();
            //OpenTray();
            StartCoroutine(RunUpgradeSequence());
            //ToolObject.GetComponent<CollectInteractable>().int_data = 0;
            //ToolObject.GetComponent<CollectInteractable>().enabled = true;
            Debug.Log("TOOL  DELIVERED");
        }
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
