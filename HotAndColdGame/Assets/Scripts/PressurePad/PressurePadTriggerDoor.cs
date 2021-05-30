using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadTriggerDoor : PressurePadTrigger
{
    public float DoorOpenTime;
    //public int direction = 1;
    private bool open;
    public Transform openPos;

    // Start is called before the first frame update
    void Start()
    {
        //openPos = transform.localPosition;
        //openPos.x -= 1.5f;

    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Trigger()
    {
        base.Trigger();
        if(!open)
        {
            StartCoroutine(Open(transform, openPos.position, DoorOpenTime));
            open = true;
        }
       
        
    }

    public IEnumerator Open(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

}
