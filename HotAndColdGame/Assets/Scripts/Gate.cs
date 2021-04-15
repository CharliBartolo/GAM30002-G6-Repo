using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IRemoteFunction
{
    public float speed;
    private float timer = 0;

    private Vector3 closedPos;
    // Start is called before the first frame update
    void Start()
    {
        closedPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        if(transform.localPosition.y < 0.6f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
    }

    public void Close()
    {
        if (transform.localPosition.y > closedPos.y)
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
    }

    public void RemoteControl()
    {
        Open();
    }
}
