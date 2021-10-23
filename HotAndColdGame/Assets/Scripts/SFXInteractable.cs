using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This class is responsible for controlling the bevahiour of interactable SFX objects in the game.
/// If the player touches the SFX, it will dissappear and reappear when the player dies unless told otherwise.
/// Latest Change: Tweaked so that when picking up a jounal, the SFX will only respawn if the journal has also been picked up.
/// By: Charadey 23/10/2021
/// </summary>
public class SFXInteractable: MonoBehaviour
{
    [SerializeField]  
    public GameObject InteractingObject;
    private bool IsInteracted;
    public bool CanPickup;
    public GameObject PickUpItem;
    public DeathEffect DE;
    private void Awake()
    {
        if (CanPickup)
        {
            if (PickUpItem == null)
            {
                Debug.LogWarning("Pick up item refrence is null despite CanPickup being set to true.");
            }
        }
        else
        {
            PickUpItem = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DE != null)
        {
            if (DE.isResetting)
            {
                if (CanPickup)
                {
                    if (PickUpItem != null)
                    {
                        gameObject.GetComponent<VisualEffect>().enabled = true;
                    }
                }
                else
                {
                    if (!IsInteracted)
                    {
                        gameObject.GetComponent<VisualEffect>().enabled = true;
                    }
                }
                
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(InteractingObject != null)
        {
            if (other == InteractingObject.GetComponent<Collider>())
            {
                IsInteracted = true;
                gameObject.GetComponent<VisualEffect>().enabled = false;
            }
        }
    }
}
