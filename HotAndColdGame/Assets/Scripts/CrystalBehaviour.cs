using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBehaviour : TemperatureStateBase
{
    public Collider crystalTemperatureArea;
    public PhysicMaterial icyPhysicMaterial;
    public Light areaLight;
    public Material coldTempField, hotTempField;
    public List<GameObject> objectsInTempArea;

    //private float powerDownRate = 0.0333f;  //Operates on a 0-1 percentage basis, Default value 0.0333 takes roughly 30 seconds from max to power down    
    protected float temperatureValueToEmit = 10f;
    // Create Use Interactable Here
    [SerializeField]private bool isPowered = true;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();        

        PerformTemperatureBehaviour(currentTempState);        
    }

    // tempMaxValue is the max / min temperature value to calculate percentage and range with
    

    protected override void PerformTemperatureBehaviour(TempState currentTemperatureState)
    {
        if (isPowered)
        {
            switch (currentTemperatureState)
            {
                case (TempState.Cold):
                    ApplyTemperatureToOtherObjects(-temperatureValueToEmit);
                    areaLight.color = new Color (62, 219, 236, 150f);
                    areaLight.enabled = true;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().material = coldTempField;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                    SpreadIceToArea();
                    break;
                case (TempState.Hot):
                    ApplyTemperatureToOtherObjects(temperatureValueToEmit);
                    areaLight.color = new Color (236, 51, 56, 150f);
                    areaLight.enabled = true;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().material = hotTempField;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                    SpreadUpdraftToArea();
                    break;
                default:
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = false;
                    areaLight.enabled = false;
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
        if (!objectsInTempArea.Contains(other.gameObject))
        {
            objectsInTempArea.Add(other.gameObject);
        }
        
    }

    protected virtual void OnTriggerExit(Collider other) 
    {
        if (objectsInTempArea.Contains(other.gameObject))
        {
            // Remove ice physic material 
            other.GetComponent<Collider>().material = null;
            
            objectsInTempArea.Remove(other.gameObject);
        }        
    }

    protected virtual void ApplyTemperatureToOtherObjects(float temperatureValueParam)
    {
        foreach (GameObject temperatureObject in objectsInTempArea)
        {
            if (temperatureObject.GetComponent<TemperatureStateBase>() != null)
            {
                temperatureObject.GetComponent<TemperatureStateBase>().ChangeTemperature(temperatureValueParam * Time.deltaTime);
            }
        }
    }

    protected virtual void SpreadUpdraftToArea()
    {
        // If object has rigidbody component, apply upward force to it as long as it remains in area
        foreach (GameObject temperatureObject in objectsInTempArea)
        {
            if (temperatureObject.GetComponent<Rigidbody>() != null)
            {
                temperatureObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Acceleration);
            }
            else if (temperatureObject.GetComponent<CharacterController>() != null)
            {
                Vector3 charYVelocity = new Vector3 (0f, temperatureObject.GetComponent<CharacterController>().velocity.y + 10f * Time.deltaTime, 0f);                
                temperatureObject.GetComponent<CharacterController>().Move(charYVelocity);
            }
        }
    }

    protected virtual void SpreadIceToArea()
    {
        /*  1. If object with TemperatureStateBase script, increment object's cold value
            2. Also change physic material to slippery
            3. Do something specific for player too
        */
        foreach (GameObject temperatureObject in objectsInTempArea)
        {
            if (temperatureObject.GetComponent<Collider>() != null)
            {
                temperatureObject.GetComponent<Collider>().material = icyPhysicMaterial;
                temperatureObject.GetComponent<Collider>().material.dynamicFriction = 0.05F;
                temperatureObject.GetComponent<Collider>().material.staticFriction = 0.05F;
                temperatureObject.GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            }
        }
    }
}
