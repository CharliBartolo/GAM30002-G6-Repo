using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerFPControls controls;    
    public float movementSpeed = 50f;

    //Mouse Control Variables
    public Camera playerCam;
    public Vector2 mouseSensitivity = new Vector2 (2, 2);
    public Vector2 mouseSmoothing = new Vector2 (3,3);
    
    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;
    //private float mouseX;
    //private float mouseY;

    private CharacterController playerController;

    //private Vector2 currentMovementInput;

    private void Awake() 
    {   
        playerController = GetComponent<CharacterController>();
        controls = new PlayerFPControls();
        //controls.Player.Look.performed += context => OnLook(context);
        controls.Enable();
    }

    private void FixedUpdate() 
    {        
        if (controls.Player.Movement.ReadValue<Vector2>().magnitude > 0)
        {
            //Debug.Log("Player registered as holding buttons");
            MovePlayer(controls.Player.Movement.ReadValue<Vector2>());
        }        
    }

    private void Update() 
    {
        MouseLook(controls.Player.Look.ReadValue<Vector2>());
    }
    
    
    // public void OnLook(InputAction.CallbackContext context)
    // {
    // }

    public void MovePlayer(Vector2 stickMovementVector)
    {
        // Translate 2d analog movement to 3d vector movement
        //Debug.Log(stickMovementVector);
        Vector3 movementVector = new Vector3 (stickMovementVector.x, 0f, stickMovementVector.y);
        movementVector = transform.TransformDirection(movementVector).normalized;
        playerController.Move(movementVector * Time.deltaTime * movementSpeed);
    }

    public void MouseLook(Vector2 deltaParam)
    {
        //Mouse Look Clamp values
        const float MIN_X = 0.0f;
        const float MAX_X = 360.0f;
        const float MIN_Y = -90.0f;
        const float MAX_Y = 90.0f;

        Vector2 mouseDelta = deltaParam;
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2 (mouseSensitivity.x * mouseSmoothing.x / 10f, mouseSensitivity.y * mouseSmoothing.y / 10f));

        // Interpolate mouse movement over time to smooth delta
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / mouseSmoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / mouseSmoothing.y);

        _mouseAbsolute += _smoothMouse;

        // Manages and clamps X axis rotation        
        if (_mouseAbsolute.x < MIN_X)
            _mouseAbsolute.x += MAX_X;
        else if (_mouseAbsolute.x > MAX_X)
            _mouseAbsolute.x -= MAX_X;

        // Manages and clamps Y axis rotation
        if (_mouseAbsolute.y < MIN_Y)
            _mouseAbsolute.y = MIN_Y;
        else if (_mouseAbsolute.y > MAX_Y)
            _mouseAbsolute.y = MAX_Y;
        
        transform.rotation = Quaternion.Euler(0f, _mouseAbsolute.x, 0f);
        playerCam.transform.rotation = Quaternion.Euler(-_mouseAbsolute.y, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
