using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCurve : MonoBehaviour
{
    public Camera cam;
    public AnimationCurve jumpCurve;
    public AnimationCurve landCurve;
    public float lerpTime = 3f;
    public float multiplier = 3f;
    private float _timer = 0f;

    IEnumerator Jump(){
        Vector3 camposstart = cam.transform.position;
        if (_timer > lerpTime)
        {
            _timer = lerpTime;
            
        }
        for(_timer = 0; _timer < lerpTime; _timer += Time.deltaTime){
        float lerpRatio = _timer / lerpTime;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 1 + jumpCurve.Evaluate(lerpRatio) * multiplier,transform.position.z);
        yield return null;
        }
       
    }

     IEnumerator Land(){
        Vector3 camposstart = cam.transform.position;
        if (_timer > lerpTime)
        {
            _timer = lerpTime;
            
        }
        for(_timer = 0; _timer < lerpTime; _timer += Time.deltaTime){
        float lerpRatio = _timer / lerpTime;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 1 + landCurve.Evaluate(lerpRatio) * multiplier,transform.position.z);
        yield return null;
        }  
    }
}
