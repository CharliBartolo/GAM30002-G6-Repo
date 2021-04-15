using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoopDemo : MonoBehaviour
{
    public bool isMovingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (transform.position.x > 5)
            {
                isMovingRight = false;
            }
            else
            {
                transform.Translate(new Vector3 (0.03f, 0f, 0f));
            }
        }
        else
        {
            if (transform.position.x < -1)
            {
                isMovingRight = true;
            }
            else
            {
                transform.Translate(new Vector3 (-0.03f, 0f, 0f));
            }
        }
    }
}
