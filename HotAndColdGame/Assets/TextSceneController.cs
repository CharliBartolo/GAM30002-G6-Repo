using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSceneController : MonoBehaviour
{
    public Animator textAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
