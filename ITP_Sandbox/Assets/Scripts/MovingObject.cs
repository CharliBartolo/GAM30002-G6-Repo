using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class MovingObject : MonoBehaviour
{
    public bool _enabled;
    private Vector3 startPos;
    private Vector3 targetPos;
    public Vector3 start;
    public Vector3 end;
    public Vector3 diff;
    public float speed;
    public bool loop;

    private float timer;
    private float percent;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        startPos = start;
        targetPos = end;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled)
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
            timer += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, timer);
        }
    }

    public void PerformMove()
    {
        //MoveToPosition(transform, targetPos, speed);
       
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
