using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCrystalBehaviour : TemperatureStateBase
{
    public Collider crystalTemperatureArea;
    public Material coldTempField, hotTempField;
    public List<GameObject> objectsInTempArea;
    public bool isPermanentlyPowered = false;

    //private float powerDownRate = 0.0333f;  //Operates on a 0-1 percentage basis, Default value 0.0333 takes roughly 30 seconds from max to power down
    private float powerDownRateInSeconds = 30f;
    private float temperatureValueToEmit = 10f;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!isPermanentlyPowered)
        {
            if (currentTemp > 0)
            {
                PowerDownToNeutral(tempMax);
            }
            else if (currentTemp < 0)
            {
                PowerDownToNeutral(tempMin);
            }
        }   

        PerformTemperatureBehaviour(currentTempState);        
    }

    // tempMaxValue is the max / min temperature value to calculate percentage and range with
    private void PowerDownToNeutral(float tempCap)
    {
        // If temperature is not zero, begin approaching neutral by ticking down current temperature until it hits neutral.
        float tempPercent = currentTemp / (tempCap - tempNeutral);   // Ranges from 0 - 1
        Debug.Log(tempPercent);
        // Rearrange equation for currentTemp while adjusting hot percent
        currentTemp = (tempPercent - (1 / powerDownRateInSeconds * Time.deltaTime)) * (tempCap - tempNeutral);

        if (tempNeutral < tempCap) 
            currentTemp = Mathf.Clamp(currentTemp, tempNeutral, tempCap);
        else
            currentTemp = Mathf.Clamp(currentTemp, tempCap, tempNeutral);    

    }

    protected override void PerformTemperatureBehaviour(TempState currentTemperatureState)
    {
        switch (currentTemperatureState)
        {
            case (TempState.Cold):
                ApplyTemperatureToOtherObjects(-temperatureValueToEmit);
                crystalTemperatureArea.GetComponent<MeshRenderer>().material = coldTempField;
                crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                SpreadIceToArea();
                break;
            case (TempState.Hot):
                ApplyTemperatureToOtherObjects(temperatureValueToEmit);
                crystalTemperatureArea.GetComponent<MeshRenderer>().material = hotTempField;
                crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                SpreadUpdraftToArea();
                break;
            default:
                crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = false;
                break;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!objectsInTempArea.Contains(other.gameObject))
        {
            objectsInTempArea.Add(other.gameObject);
        }
        
    }

    private void OnTriggerExit(Collider other) 
    {
        if (objectsInTempArea.Contains(other.gameObject))
        {
            // Remove ice physic material 
            other.GetComponent<Collider>().material = null;
            
            objectsInTempArea.Remove(other.gameObject);
        }        
    }

    private void ApplyTemperatureToOtherObjects(float temperatureValueParam)
    {
        foreach (GameObject temperatureObject in objectsInTempArea)
        {
            if (temperatureObject.GetComponent<TemperatureStateBase>() != null)
            {
                temperatureObject.GetComponent<TemperatureStateBase>().ChangeTemperature(temperatureValueParam * Time.deltaTime);
            }
        }
    }

    private void SpreadUpdraftToArea()
    {
        // If object has rigidbody component, apply upward force to it as long as it remains in area
        foreach (GameObject temperatureObject in objectsInTempArea)
        {
            if (temperatureObject.GetComponent<Rigidbody>() != null)
            {
                temperatureObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 40f, ForceMode.Force);
            }
        }
    }

    private void SpreadIceToArea()
    {
        /*  1. If object with TemperatureStateBase script, increment object's cold value
            2. Also change physic material to slippery
            3. Do something specific for player too
        */
        foreach (GameObject temperatureObject in objectsInTempArea)
        {
            if (temperatureObject.GetComponent<Collider>() != null)
            {
                temperatureObject.GetComponent<Collider>().material = new PhysicMaterial();
                temperatureObject.GetComponent<Collider>().material.dynamicFriction = 0.05F;
                temperatureObject.GetComponent<Collider>().material.staticFriction = 0.05F;
                temperatureObject.GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            }
        }
    }
}
