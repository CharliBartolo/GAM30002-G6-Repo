using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] public Gate controlled;
    private LightDetection detector;
    private bool active;


    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponent<LightDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        active = detector.isLit;

        if(active)
        {
            OpenGate();
        }else
        {
            CloseGate();
        }
    }

    public void OpenGate()
    {
        controlled.GetComponent<Gate>().Open();
    }

    public void CloseGate()
    {
        controlled.GetComponent<Gate>().Close();
    }
}
