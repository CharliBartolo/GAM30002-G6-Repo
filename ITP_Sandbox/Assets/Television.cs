using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnState { ON, OFF }
public class Television : MonoBehaviour, IRemoteFunction
{
    public OnState state;
    public GameObject screen;
    public GameObject lightBeam;

    public Material[] _channels;
    public Material _off;
    public Material _on;

    public bool performed;


    // Start is called before the first frame update
    void Start()
    {
        TurnOff();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void IRemoteFunction.RemoteControl()
    {
        ChangeState();
        if (!performed)
        {

        }

        performed = true;
        Invoke("EnablePerformance", 0.2f);
    }

    void ChangeState()
    {
        switch (state)
        {
            case OnState.ON:
                TurnOff();
                break;

            case OnState.OFF:
                TurnOn();
                break;
        }
    }

    void EnablePerformance()
    {
        performed = false;
    }
    public void TurnOn()
    {
        state = OnState.ON;
        //screen.GetComponent<Renderer>().material = _channels[Random.Range(0, _channels.Length)];
        screen.GetComponent<Renderer>().material = _on;
        lightBeam.SetActive(true);
    }

    public void TurnOff()
    {
        state = OnState.OFF;
        screen.GetComponent<Renderer>().material = _off;
        lightBeam.SetActive(false);
    }
}
