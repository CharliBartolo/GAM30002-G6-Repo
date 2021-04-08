using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class MovingObject : MonoBehaviour
{
    [SerializeField] public bool _enabled;
    private Vector3 startPos;
    private Vector3 targetPos;
    public Vector3 start;
    public Vector3 end;
    public float speed;
    public float duration;

    private float timer;
    private float percent;

    private bool canMove;
    public bool moving;

    // Start is called before the first frame update
    void Start()
    {
        startPos = start;
        targetPos = end;

        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            Debug.Log("elevetor performed move 1");
            canMove = false;
            timer += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, timer);
            Debug.Log("elevetor performed move 2");

            if (transform.position == start)
            {
                moving = false;
                startPos = start;
                targetPos = end;
                timer = 0;
            }
            if (transform.position == end)
            {
                moving = false;
                startPos = end;
                targetPos = start;
                timer = 0;
            }
            Debug.Log("elevetor performed move 3");
        }

       /* if (_enabled)
        {
            //PerformMove();
            if (transform.position == start)
            {
                startPos = start;
                targetPos = end;
                timer = 0;
            }
            else if (transform.position == end)
            {
                startPos = end;
                targetPos = start;
                timer = 0;
            }
          
        }*/
    }

    public void PerformMove()
    {
        
        if (!moving)
        {
            moving = true;
            Debug.Log("elevetor performing move");
            if (transform.position == start)
            {
                startPos = start;
                targetPos = end;
                timer = 0;
            }
            else if (transform.position == end)
            {
                startPos = end;
                targetPos = start;
                timer = 0;
            }
        }

    }

    public void MoveToStart()
    {
        _enabled = true; 
        startPos = end;
        targetPos = start;
        timer = 0;
    }

    public void MoveToEnd()
    {
        _enabled = true;
        startPos = start;
        targetPos = end;
        timer = 0;
    }

    public void MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, timeToMove * Time.deltaTime);
    }

    public void SetStartPosition()
    {
        start = transform.position;
    }
    public void SetEndPosition()
    {
        end = transform.position;
    }

}
