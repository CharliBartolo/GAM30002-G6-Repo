using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CrystalBehaviour : TemperatureStateBase
{
    //public HDAdditionalLightData bluelight;
    //public HDAdditionalLightData redlight;
    //float r = 0;
    //float b = 0;
    //float g = 0;
    //public Color col;
    //public float col_intensity_rate = 10;
    public Material mat;
    //public GameObject mat_obj; //make into array
    //public float light_min = 0;
    //public float light_max = 10000;
    //public float intensity_rate = 10;
    public Collider crystalTemperatureArea;
    //public Collider crystalShootableCollider;
    public PhysicMaterial icyPhysicMaterial;
    public Light areaLight;
    public Material coldTempField, hotTempField;
    public Dictionary<GameObject, PhysicMaterial> objectsInTempArea;

    //private float powerDownRate = 0.0333f;  //Operates on a 0-1 percentage basis, Default value 0.0333 takes roughly 30 seconds from max to power down    
    public float temperatureValueToEmit = 5f;
    // Create Use Interactable Here
    [SerializeField] private bool isPowered = true;    
    [SerializeField] public bool spreadEffects = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        //mat = mat_obj.GetComponent<Renderer>().material;
        base.Start();
        objectsInTempArea = new Dictionary<GameObject, PhysicMaterial>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PerformTemperatureBehaviour(CurrentTempState);
        //col = new Color(r, g, b);
        //Color col2 = col - new Color(20,20,20);
        //mat.SetColor("Color_ea2f6e2b682b42c99f990beacb20c9fa", col * 1);
        //mat.SetColor("Color_f0defd60fc814840bf788457eddb279f", col2 * 1);
    }

    // tempMaxValue is the max / min temperature value to calculate percentage and range with


    public override void PerformTemperatureBehaviour(ITemperature.tempState currentTemperatureState)
    {
        // if (isPowered)
        // {
        //     switch (currentTemperatureState)
        //     {
        //         case (ITemperature.tempState.Cold):
        //             ApplyTemperatureToOtherObjects(-temperatureValueToEmit);
        //             SpreadIceToArea();
        //             break;

        //         case (ITemperature.tempState.Hot):
        //             ApplyTemperatureToOtherObjects(temperatureValueToEmit);
        //             SpreadLowGravToArea();
        //             break;
        //         default:

        //             //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = false;
        //             foreach (GameObject temperatureObject in objectsInTempArea.Keys)
        //             {
        //                 if (temperatureObject.GetComponent<Collider>() != null)
        //                 {
        //                     temperatureObject.GetComponent<Collider>().material = null;                            
        //                 }
        //             }
        //             break;
        //     }
        // }

        if (isPowered)
        {
            if (currentTemp < tempValueRange[1])
            {
                    ApplyTemperatureToOtherObjects(-temperatureValueToEmit);
                if(spreadEffects)
                    SpreadIceToArea();
            }
            else if (currentTemp > tempValueRange[1])
            {
                    ApplyTemperatureToOtherObjects(temperatureValueToEmit);
                if(spreadEffects)
                {
                    ResetIce();
                    SpreadLowGravToArea();
                }
                
            }
            else
            {
                if(spreadEffects)
                    ResetIce();
            }
        }                       
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!objectsInTempArea.ContainsKey(other.gameObject))
        {
            if(!other.gameObject.GetComponent<MachineFXController>() && (other.gameObject.GetComponent<Rigidbody>() != null || 
                other.gameObject.GetComponent<ITemperature>() != null || other.gameObject.GetComponent<IConditions>() != null))
                objectsInTempArea.Add(other.gameObject, other.material);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (objectsInTempArea.ContainsKey(other.gameObject))
        {
            // Remove ice physic material 
            other.GetComponent<Collider>().material = objectsInTempArea[other.gameObject];

            objectsInTempArea.Remove(other.gameObject);
        }
    }
    //ADDED: speed modifier
    protected virtual void ApplyTemperatureToOtherObjects(float temperatureValueParam)
    {
        foreach (GameObject temperatureObject in objectsInTempArea.Keys)
        {
            //Added proximity
            if (temperatureObject.GetComponent<ITemperature>() != null)
            {
                if (temperatureObject.GetComponent<PlayerController>() != null)
                {
                    if (spreadEffects)
                    {
                        float distance = Vector3.Distance(transform.position, temperatureObject.GetComponent<Transform>().position);
                        //float multiplier = 1.0F - Mathf.Clamp01(distance / (GetComponent<SphereCollider>().radius * transform.localScale.x));
                        float multiplier = 1.0F;
                        temperatureObject.GetComponent<ITemperature>().ChangeTemperature(multiplier * temperatureValueParam);
                    }
                    else
                    {
                        float temp = temperatureObject.GetComponent<ITemperature>().CurrentTemperature;
                        // option caps temp to a power level
                        if ((CurrentTemperature > 0 && temp < currentTemp))
                        {
                            float distance = Vector3.Distance(transform.position, temperatureObject.GetComponent<Transform>().position);
                            //float multiplier = 1.0F - Mathf.Clamp01(distance / (GetComponent<SphereCollider>().radius * transform.localScale.x));
                            float multiplier = 1.0F;
                            temperatureObject.GetComponent<ITemperature>().ChangeTemperature(multiplier * temperatureValueParam);
                        }
                        else if ((CurrentTemperature < 0 && temp > currentTemp))
                        {
                            float distance = Vector3.Distance(transform.position, temperatureObject.GetComponent<Transform>().position);
                            //float multiplier = 1.0F - Mathf.Clamp01(distance / (GetComponent<SphereCollider>().radius * transform.localScale.x));
                            float multiplier = 1.0F;
                            temperatureObject.GetComponent<ITemperature>().ChangeTemperature(multiplier * temperatureValueParam);
                        }
                    }
                }
                else
                {
                    temperatureObject.GetComponent<ITemperature>().ChangeTemperature(temperatureValueParam * Time.deltaTime);
                }
            }
        }
    }

    protected virtual void SpreadLowGravToArea()
    {
        // Could differentiate player / key items and random items via temp / condition system?

        // If object has rigidbody component, apply upward force to it as long as it remains in area
        foreach (GameObject temperatureObject in objectsInTempArea.Keys)
        {
            if (temperatureObject.GetComponent<IConditions>() != null)
            {
                temperatureObject.GetComponent<IConditions>().AddCondition(IConditions.ConditionTypes.ConditionHot);
            }
            else if (temperatureObject.GetComponent<Rigidbody>() != null)
            {
                temperatureObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity * 25f * Time.deltaTime, ForceMode.Acceleration);
            }            
        }
    }

    protected virtual void SpreadIceToArea()
    {
        /*  1. If object with TemperatureStateBase script, increment object's cold value
            2. Also change physic material to slippery
            3. Do something specific for player too
        */
        foreach (GameObject temperatureObject in objectsInTempArea.Keys)
        {
            if (temperatureObject.GetComponent<IConditions>() != null)
            {
                temperatureObject.GetComponent<IConditions>().AddCondition(IConditions.ConditionTypes.ConditionCold);
            }
            else if (temperatureObject.GetComponent<Collider>() != null && temperatureObject.GetComponent<Rigidbody>() != null)
            {
                temperatureObject.GetComponent<Collider>().material = icyPhysicMaterial;
                //temperatureObject.GetComponent<Collider>().material.dynamicFriction = 0.05F;
                //temperatureObject.GetComponent<Collider>().material.staticFriction = 0.05F;
                //temperatureObject.GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            }

            //Added IConditions
  
        }
    }

    private void ResetIce()
    {
        foreach (GameObject temperatureObject in objectsInTempArea.Keys)
        {
            if (temperatureObject.GetComponent<Collider>() != null)
            {
                temperatureObject.GetComponent<Collider>().material = null;                            
            }                    
        } 
    }
}

