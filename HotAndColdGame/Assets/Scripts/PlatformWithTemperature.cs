using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWithTemperature : TemperatureStateBase
{
    [SerializeField] public TemperatureStateBase Trigger = null;

    protected Transform origin; // the start target position
    protected Transform coldTarget; // the cold target position
    protected Transform hotTarget; // the cold target position
    protected GameObject platformObj;
    
    public float speed; // speed - units per second (gives you control of how fast the object will move in the inspector)
    public float Delay = 1;
    public bool canMove = true; // a public bool that allows you to toggle this script on and off in the inspector

    private bool movingToPosition;
<<<<<<< HEAD
    private bool returningToOrigin;
=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6

    private Queue<Vector3> pos = new Queue<Vector3>();
    private Vector3 targetPos = Vector3.zero;

    private Material emissiveMaterial;

    protected override void Start()
    {
        base.Start();

        emissiveMaterial = GameMaster.instance.colourPallete.materials.EmissiveLights;


        Renderer[] r = transform.GetChild(0).GetComponentsInChildren<Renderer>();

        foreach (var item in r)
        {
            item.sharedMaterial = new Material(emissiveMaterial);
        }

        origin = transform.parent.transform.Find("Positions").transform.Find("OriginPos");
        coldTarget = transform.parent.transform.Find("Positions").transform.Find("ColdPos");
        hotTarget = transform.parent.transform.Find("Positions").transform.Find("HotPos");
        platformObj = this.gameObject;


        if(Trigger)
            SetLights(GameMaster.instance.colourPallete.Neutral);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    void Update()
    {

        if (canMove && Trigger != null )
        {
            float step = speed * Time.deltaTime; // step size = speed * frame time

<<<<<<< HEAD
            if(returningToOrigin)
            {
                if (transform.position == origin.position)
                {
                    returningToOrigin = false;
                }

                Debug.Log("RETURNING TO ORIGIN");
                targetPos = origin.transform.position;
                Trigger.SetTemperature(0);
                pos.Clear();

                SetLights(GameMaster.instance.colourPallete.Neutral);

               
            }


=======
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
            // enque target positions based on state
            switch (Trigger.CurrentTempState)
            {
                case ITemperature.tempState.Cold:

                    if (transform.position == origin.position)
<<<<<<< HEAD
                    {
                        if (!pos.Contains(coldTarget.transform.position))
                            pos.Enqueue(coldTarget.transform.position);
                    }
                    else if (transform.position != coldTarget.transform.position && targetPos != coldTarget.transform.position)
                    {
                        returningToOrigin = true;
=======
                    {
                        if (!pos.Contains(coldTarget.transform.position))
                            pos.Enqueue(coldTarget.transform.position);
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
                    }

                    break;

                case ITemperature.tempState.Hot:

                    if (transform.position == origin.position)
<<<<<<< HEAD
                    {
                        if (!pos.Contains(hotTarget.transform.position))
                            pos.Enqueue(hotTarget.transform.position);
                    }
                    else if (transform.position != hotTarget.transform.position && targetPos != hotTarget.transform.position)
                    {
                        returningToOrigin = true;
=======
                    {
                        if (!pos.Contains(hotTarget.transform.position))
                            pos.Enqueue(hotTarget.transform.position);
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
                    }

                    break;

                case ITemperature.tempState.Neutral:

                    if (!pos.Contains(origin.transform.position))
                        pos.Enqueue(origin.transform.position);

                    break;
            }

            // dequeue next position
<<<<<<< HEAD
            if (pos.Count > 0)
            {
                targetPos = pos.Dequeue();              
=======
            //if (targetPos == Vector3.zero || (platformObj.transform.position == targetPos && pos.Count > 0))
            if (targetPos == Vector3.zero || ( pos.Count > 0))
            {
                targetPos = pos.Dequeue();
               
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
            }

            // move towards target
            if (targetPos != Vector3.zero)
                platformObj.transform.position = Vector3.MoveTowards(platformObj.transform.position, targetPos, step);

<<<<<<< HEAD
            // set lights

           if (targetPos == coldTarget.transform.position)
=======
            // set lights based on state
            if (targetPos == origin.transform.position)
            {
                SetLights(GameMaster.instance.colourPallete.Neutral);
            }
            else if (targetPos == coldTarget.transform.position)
>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
            {
                SetLights(GameMaster.instance.colourPallete.Negative);
            }
            else if (targetPos == hotTarget.transform.position)
            {
                SetLights(GameMaster.instance.colourPallete.Positive);
            }
<<<<<<< HEAD
            else if (targetPos == origin.transform.position)
            {
                Debug.Log("SETTING COLOUR NEUTRAL");
                //SetLights(GameMaster.instance.colourPallete.Neutral);
                SetLights(Color.clear);
            }
=======

            Debug.Log("TARGET POSITION: " + targetPos);

>>>>>>> 7b688233387786860c4dc5b974fab5d75dd2dbe6
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
