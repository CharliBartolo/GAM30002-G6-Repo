using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableBase : MonoBehaviour
{
    //Carry = Pick up and move with you, follows mouse
    //RotateOnly = Uses mouse delta to rotate object instead of moving camera
    //Use = Use and that's it, e.g. a button, holding Interact does nothing.
    public enum InteractionType {Carry, RotateOnly, Use}; 
    
    public abstract void OnInteractEnter(PlayerInput playerInputRef);        

    public abstract void OnInteracting();
    public abstract void OnInteractExit();
    
    public abstract InteractionType pInteractionType {get; set;}
    public abstract PlayerInput pPlayerInput {get; set;}
}
