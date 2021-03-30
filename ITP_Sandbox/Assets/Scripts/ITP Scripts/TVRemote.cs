using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVRemote : MonoBehaviour, IRemoteFunction
{
    public GameObject controlled;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void IRemoteFunction.RemoteControl()
    {
        controlled.GetComponent<IRemoteFunction>().RemoteControl();
    }
}
