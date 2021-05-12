using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBehaviour : TemperatureStateBase
{
    public Collider crystalTemperatureArea;
    //public Collider crystalShootableCollider;
    public PhysicMaterial icyPhysicMaterial;
    public Light areaLight;
    public Material coldTempField, hotTempField;
    public Dictionary<GameObject, PhysicMaterial> objectsInTempArea;

    //private float powerDownRate = 0.0333f;  //Operates on a 0-1 percentage basis, Default value 0.0333 takes roughly 30 seconds from max to power down    
    public float temperatureValueToEmit = 5f;
    // Create Use Interactable Here
    [SerializeField]private bool isPowered = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        objectsInTempArea = new Dictionary<GameObject, PhysicMaterial>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();        

        PerformTemperatureBehaviour(CurrentTempState);        
    }

    // tempMaxValue is the max / min temperature value to calculate percentage and range with
    

    public override void PerformTemperatureBehaviour(ITemperature.tempState currentTemperatureState)
    {
        if (isPowered)
        {
            switch (currentTemperatureState)
            {
                case (ITemperature.tempState.Cold):
                    ApplyTemperatureToOtherObjects(-temperatureValueToEmit);
                    //areaLight.color = new Color (62, 219, 236, 150f);
                    areaLight.enabled = true;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().material = coldTempField;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                    SpreadIceToArea();
                    break;
                case (ITemperature.tempState.Hot):
                    ApplyTemperatureToOtherObjects(temperatureValueToEmit);
                    //areaLight.color = new Color (236, 51, 56, 150f);
                    areaLight.enabled = true;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().material = hotTempField;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                    SpreadLowGravToArea();
                    break;
                default:
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = false;
                    areaLight.enabled = false;
                    foreach (GameObject temperatureObject in objectsInTempArea.Keys)                    
                    {
                        if (temperatureObject.GetComponent<Collider>() != null)
                        {
                            temperatureObject.GetComponent<Collider>().material = null;
                            //temperatureObject.GetComponent<Collider>().material.dynamicFriction = 0.05F;
                            //temperatureObject.GetComponent<Collider>().material.staticFriction = 0.05F;
                            //temperatureObject.GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
                        }
                    }
                    break;
            }
        }
        else
        {
            areaLight.enabled = false;
        }
        
    }

    protected virtual void OnTriggerEnter(Collider other) 
    {
        if (!objectsInTempArea.ContainsKey(other.gameObject))
        {
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
            if (temperatureObject.GetComponent<ITemperature>() != null)
            {
                temperatureObject.GetComponent<ITemperature>().ChangeTemperature(temperatureValueParam * Time.deltaTime);
            }
        }
    }

    protected virtual void SpreadLowGravToArea()
    {
        // If object has rigidbody component, apply upward force to it as long as it remains in area
        foreach (GameObject temperatureObject in objectsInTempArea.Keys)
        {
            if (temperatureObject.GetComponent<Rigidbody>() != null)
            {
                temperatureObject.GetComponent<Rigidbody>().AddForce(Physics.gravity * -0.3f, ForceMode.Acceleration);
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
            if (temperatureObject.GetComponent<Collider>() != null)
            {
                temperatureObject.GetComponent<Collider>().material = icyPhysicMaterial;
                //temperatureObject.GetComponent<Collider>().material.dynamicFriction = 0.05F;
                //temperatureObject.GetComponent<Collider>().material.staticFriction = 0.05F;
                //temperatureObject.GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            }
        }
    }
}
