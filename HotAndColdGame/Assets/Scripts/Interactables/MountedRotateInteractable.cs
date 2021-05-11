using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MountedRotateInteractable : InteractableBase
{
    public Vector2 mouseSensitivity = new Vector2 (2, 2);
    public Vector2 mouseSmoothing = new Vector2 (3,3);

    //Should probably change these to lists, not vectors
    public Vector2 xAngleClamps = new Vector2 (0f, 360f);
    public Vector2 yAngleClamps = new Vector2 (-90f, 90f); 

    private Vector2 _mouseSmooth;
    private Vector2 _mouseAbsolute;
    private InteractionType interactionType = InteractionType.RotateOnly;
    private PlayerInput playerInput;

    private void Start() 
    {
        // Ensures clamps and movement consider starting object rotation
        _mouseAbsolute.x = transform.rotation.eulerAngles.z; 
        _mouseAbsolute.y = -transform.rotation.eulerAngles.x;
        xAngleClamps = new Vector2 (xAngleClamps.x + _mouseAbsolute.x, xAngleClamps.y + _mouseAbsolute.x);
        yAngleClamps = new Vector2 (yAngleClamps.x + _mouseAbsolute.y, yAngleClamps.y + _mouseAbsolute.y);        
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef)
    {
        pPlayerInput = playerInputRef;
    }

    //Runs when interaction ceases
    public override void OnInteractExit()
    {
        pPlayerInput = null;
    }

    //Runs every frame the interaction continues
    public override void OnInteracting()
    {       
        RotateObject();
    }

    void RotateObject()
    {
        _mouseAbsolute += MouseSmooth(playerInput.actions.FindAction("Look").ReadValue<Vector2>());
        MouseClamp();
        
        transform.rotation = Quaternion.Euler(-_mouseAbsolute.y, _mouseAbsolute.x, 0f);        
    }

    Vector2 MouseSmooth(Vector2 deltaParam)
    {
        Vector2 mouseDelta = deltaParam;
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2 (mouseSensitivity.x * mouseSmoothing.x / 10f, mouseSensitivity.y * mouseSmoothing.y / 10f));

        // Interpolate mouse movement over time to smooth delta
        _mouseSmooth.x = Mathf.Lerp(_mouseSmooth.x, mouseDelta.x, 1f / mouseSmoothing.x);
        _mouseSmooth.y = Mathf.Lerp(_mouseSmooth.y, mouseDelta.y, 1f / mouseSmoothing.y);

        return _mouseSmooth;
    }

    void MouseClamp()
    {
         // Manages and clamps X axis rotation        
        if (_mouseAbsolute.x < xAngleClamps.x)
            _mouseAbsolute.x += xAngleClamps.y;
        else if (_mouseAbsolute.x > xAngleClamps.y)
            _mouseAbsolute.x -= xAngleClamps.y;

        // Manages and clamps Y axis rotation
        if (_mouseAbsolute.y < yAngleClamps.x)
            _mouseAbsolute.y = yAngleClamps.x;
        else if (_mouseAbsolute.y > yAngleClamps.y)
            _mouseAbsolute.y = yAngleClamps.y;
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
