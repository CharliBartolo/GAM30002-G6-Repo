using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    // Mouse Control Settings   
    public Vector2 mouseSensitivity = new Vector2(1, 1);
    public Vector2 mouseSmoothing = new Vector2(2, 2);
    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;
    const float MIN_Y = -90.0f;
    const float MAX_Y = 90.0f;
    private Vector2 _mouseAbsolute;
    private Vector2 _mouseSmooth;

    // Mouse Functions below
    Vector2 MouseSmooth(Vector2 deltaParam)
    {
        Vector2 mouseDelta = deltaParam;
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2 (mouseSensitivity.x * mouseSmoothing.x / 10f, mouseSensitivity.y * mouseSmoothing.y / 10f));

        // Interpolate mouse movement over time to smooth delta
        _mouseSmooth.x = Mathf.Lerp(_mouseSmooth.x, mouseDelta.x, 1f / mouseSmoothing.x);
        _mouseSmooth.y = Mathf.Lerp(_mouseSmooth.y, mouseDelta.y, 1f / mouseSmoothing.y);

        return _mouseSmooth;
    }

    public void ResetMouse(Transform transformToMatch)
    {
        //_mouseAbsolute = Vector2.zero;
        _mouseAbsolute = new Vector2 (transformToMatch.eulerAngles.y, transformToMatch.eulerAngles.x);
    }

    private void MouseClamp()
    {
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
    }

    public void MouseLook(Vector2 deltaParam, Camera playerCam)
    {
        _mouseAbsolute += MouseSmooth(deltaParam);
        MouseClamp();

        transform.rotation = Quaternion.Euler(0f, _mouseAbsolute.x, 0f);
        playerCam.transform.rotation = Quaternion.Euler(-_mouseAbsolute.y, transform.eulerAngles.y, transform.eulerAngles.z);      
    }

    public Vector2 MouseAbsolute
    {
        get
        {
            return _mouseAbsolute;
        }
        set
        {
            _mouseAbsolute = value;
        }
    }
}
