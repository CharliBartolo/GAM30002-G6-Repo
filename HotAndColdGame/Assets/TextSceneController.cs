using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The TextSceneController script manages the skip and speed up function for the narrative text in the scrolling text scene.
/// Last edited by: Charadey - 08/10/2021
/// </summary>

public class TextSceneController : MonoBehaviour
{
    public Animator textAnimator;

    //Speed Up Text
    [Range(0, 500)]
    public float textSpeedIncrease;
    [HideInInspector]
    public bool speedToggle;

    private void Start()
    {
        speedToggle = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if (textAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("Skipping in 3s");
            Invoke("Skip", 3f);          
        }   
    }

    
    public void Skip()
    {
        GameMaster.instance.LoadNextScene();
    }

    public void Toggle()
    {
        speedToggle = !speedToggle;
        if (speedToggle)
        {
            textAnimator.speed += (textSpeedIncrease / 100f) * textAnimator.speed;
        }
        else
        {
            textAnimator.speed = ((textAnimator.speed * 100f) / (100f + textSpeedIncrease));
        }

        Debug.Log(textAnimator.speed);
    }
}
