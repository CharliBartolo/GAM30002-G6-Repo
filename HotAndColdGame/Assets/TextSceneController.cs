using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;

/// <summary>
/// The TextSceneController script manages the skip and speed up function for the narrative text in the scrolling text scene.
/// Last edited by: Charadey - 08/10/2021
/// </summary>
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6

public class TextSceneController : MonoBehaviour
{
    public Animator textAnimator;

<<<<<<< HEAD
    //Speed Up Text
    [Range(0, 500)]
    public float textSpeedIncrease;
    [HideInInspector]
    public bool speedToggle;

    private void Start()
    {
        speedToggle = false;    
=======
    // Start is called before the first frame update
    void Start()
    {
        
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    }

    // Update is called once per frame
    void Update()
    {
        if (textAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Debug.Log("Skipping in 3s");
<<<<<<< HEAD
            Invoke("Skip", 3f);          
        }   
    }

    
=======
            Invoke("Skip", 3f);
            
        }
    }

>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
    public void Skip()
    {
        GameMaster.instance.LoadNextScene();
    }
<<<<<<< HEAD

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
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
}
