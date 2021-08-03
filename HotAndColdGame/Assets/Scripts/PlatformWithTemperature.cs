using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWithTemperature : TemperatureStateBase
{
    [SerializeField] TemperatureStateBase Trigger = null;

    public Transform origin; // the start target position
    public Transform coldTarget; // the cold target position
    public Transform hotTarget; // the cold target position
    public GameObject platformObj;
    
    public float speed; // speed - units per second (gives you control of how fast the object will move in the inspector)
    public float Delay = 1;
    public bool canMove = true; // a public bool that allows you to toggle this script on and off in the inspector

    private Queue<Vector3> pos = new Queue<Vector3>();
    private Vector3 targetPos = Vector3.zero;

    private Material emissiveMaterial;

    protected override void Start()
    {
        base.Start();
        emissiveMaterial = new Material(GameMaster.instance.colourPallete.materials.EmissiveLights);
        SetLights(GameMaster.instance.colourPallete.Neutral);
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            float step = speed * Time.deltaTime; // step size = speed * frame time
            switch (Trigger.CurrentTempState)
            {
                case ITemperature.tempState.Cold:
                    if (!pos.Contains(coldTarget.transform.position))
                    {
                        pos.Enqueue(coldTarget.transform.position);
                    }
                        
                    //platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, coldTarget.transform.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Hot:

                    if (!pos.Contains(hotTarget.transform.position))
                    {
                        pos.Enqueue(hotTarget.transform.position);
                    }
                      
                    //platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, hotTarget.transform.position, step); // moves position a step closer to the target position
                    break;
                case ITemperature.tempState.Neutral:
                    if (!pos.Contains(origin.transform.position))
                        pos.Enqueue(origin.transform.position);
                    //platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, origin.transform.position, step); // moves position a step closer to the target position
                    break;
            }

            if (targetPos == Vector3.zero || (platformObj.transform.position == targetPos && pos.Count > 0))
            {
                targetPos = pos.Dequeue();
            }

            if (targetPos != Vector3.zero)
                platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, targetPos, step);


            if (targetPos == origin.transform.position)
            {
                SetLights(GameMaster.instance.colourPallete.Neutral);
            }
            else if (targetPos == coldTarget.transform.position)
            {
                SetLights(GameMaster.instance.colourPallete.Negative);
            }
            else if (targetPos == hotTarget.transform.position)
            {
                SetLights(GameMaster.instance.colourPallete.Positive);
            }

        }    
    }

    public void SetLights(Color colour)
    {
        Renderer[] r = transform.GetChild(0).GetComponentsInChildren<Renderer>();

        foreach (var item in r)
        {
            if(item.sharedMaterial.color != colour)
                item.sharedMaterial.SetColor("_EmissiveColor", colour * Trigger.GetComponent<MachineFXController>().emissionValue);
        }
    }
}
