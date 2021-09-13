using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolBox : CollectInteractable
{
    public GameObject ToolDisplayObject;
    public GameObject ToolObject;
    private GameObject currentLevelToolgun;

    public Transform ref_level0;
    public Transform ref_level1;
    public Transform ref_level2;

    public float upgradeTime;

    private bool triggered;
    //public bool destroyOnUse;

    //public string itemName;
    //private InteractionType interactionType = InteractionType.Use;
    private PlayerInput _playerInput;
    //public int gunUpgradeLevel = 0;
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();

    private void Start()
    {
        ToolDisplayObject.SetActive(true);
        ToolObject.SetActive(false);
        OpenTray();
    }

    public void PlayAudio(int clip)
    {
        AudioClip toPlay = sounds[clip];
        AudioSource.PlayClipAtPoint(toPlay, transform.position);
    }

    public void OpenTray()
    {
        PlayAudio(0);
        GetComponent<Animator>().speed = 1;
        GetComponent<Animator>().Play("Open");
    }

    public IEnumerator RunUpgradeSequence()
    {
        GetComponent<Animator>().speed = 2;
        GetComponent<Animator>().Play("Close");
        PlayAudio(0);
        PlayAudio(1);

        yield return new WaitForSeconds(upgradeTime);

        PlayAudio(0);
        GetComponent<Animator>().speed = 2;
        GetComponent<Animator>().Play("Open");
        ToolObject.GetComponent<CollectInteractable>().enabled = true;
        //ToolDisplayObject.SetActive(false);
        PlaceCurrentWeapon((int)GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.gunUpgradeState, false);
        ToolObject.SetActive(true);
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef, float delay = 0)
    {
        StartCoroutine(InteractAction(delay));
    }

    IEnumerator InteractAction(float delay)
    {
        yield return new WaitForSeconds(delay);

        RayCastShootComplete.gunUpgrade currentGunLevel = GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.gunUpgradeState;

        if ((int)currentGunLevel < 2 && GameObject.Find("Player").GetComponent<GunFXController>().equipped)
        {
            GetComponent<Collider>().enabled = false;
            //playerControls = playerControlsRef;

            //Debug.Log("TOOL REQUESTED");
            triggered = true;
            ToolDisplayObject.SetActive(false);


            PlaceCurrentWeapon((int)currentGunLevel, true);


            //ToolObject.SetActive(true);

            ToolObject.GetComponent<CollectInteractable>().enabled = false;
            //GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.SetGunUpgradeState((int)currentGunLevel + 1);
            GameObject.Find("Player").GetComponent<PlayerController>().isGunEnabled = false;
            GameObject.Find("Player").GetComponent<GunFXController>().UnEquipTool();
            //OpenTray();
            StartCoroutine(RunUpgradeSequence());
            //ToolObject.GetComponent<CollectInteractable>().int_data = 0;
            //ToolObject.GetComponent<CollectInteractable>().enabled = true;
            //Debug.Log("TOOL  DELIVERED");
        }
    }
    
    public void PlaceCurrentWeapon(int currentLevel, bool state)
    {
        switch (currentLevel)
        {
            case 0:
                //transform.Find("CollectableRaygun").gameObject.SetActive(true);
                ref_level0.gameObject.SetActive(state);
                break;
            case 1:
                //transform.Find("CollectableRaygun_NegativeOnly").gameObject.SetActive(true);
                ref_level1.gameObject.SetActive(state);
                break;
        }
    }
    public void HideCurrentWeapon(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0:
                transform.Find("CollectableRaygun").gameObject.SetActive(false);
                break;
            case 1:
                transform.Find("CollectableRaygun_NegativeOnly").gameObject.SetActive(false);
                break;
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
            return _playerInput;
        }
        set
        {
            _playerInput = value;
        }
    }


}
