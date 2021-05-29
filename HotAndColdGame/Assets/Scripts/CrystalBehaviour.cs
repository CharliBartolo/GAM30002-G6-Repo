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
        if (isPowered)
        {
            switch (currentTemperatureState)
            {
                case (ITemperature.tempState.Cold):
                    ApplyTemperatureToOtherObjects(-temperatureValueToEmit);
                    //areaLight.color = new Color (62, 219, 236, 150f);
                    //Debug.Log("intensity changing :" + bluelight.intensity);
                    //bluelight.intensity = Mathf.MoveTowards(bluelight.intensity, light_max, intensity_rate * Time.deltaTime);
                    //redlight.intensity = Mathf.MoveTowards(redlight.intensity, light_min, intensity_rate * Time.deltaTime);

                    //r = Mathf.MoveTowards(r, 0, col_intensity_rate * Time.deltaTime);
                    //g = Mathf.MoveTowards(g, 22, col_intensity_rate * Time.deltaTime);
                    //b = Mathf.MoveTowards(b, 85, col_intensity_rate * Time.deltaTime);

                    //crystalTemperatureArea.GetComponent<MeshRenderer>().material = coldTempField;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                    SpreadIceToArea();
                    break;


                    //Color2 name - Color_ea2f6e2b682b42c99f990beacb20c9fa
                    //Color1 name - Color_f0defd60fc814840bf788457eddb279f

                case (ITemperature.tempState.Hot):
                    ApplyTemperatureToOtherObjects(temperatureValueToEmit);
                    //areaLight.color = new Color (236, 51, 56, 150f);
                    //bluelight.intensity = Mathf.MoveTowards(bluelight.intensity, light_min, intensity_rate * Time.deltaTime);
                    //redlight.intensity = Mathf.MoveTowards(redlight.intensity, light_max, intensity_rate * Time.deltaTime);

                    //r = Mathf.MoveTowards(r, 75, col_intensity_rate * Time.deltaTime);
                    //g = Mathf.MoveTowards(g, 0, col_intensity_rate * Time.deltaTime);
                    //b = Mathf.MoveTowards(b, 0, col_intensity_rate * Time.deltaTime);

                    //crystalTemperatureArea.GetComponent<MeshRenderer>().material = hotTempField;
                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = true;
                    SpreadLowGravToArea();
                    break;
                default:

                    //r = Mathf.MoveTowards(r, 32, col_intensity_rate * Time.deltaTime);
                    //g = Mathf.MoveTowards(g, 32, col_intensity_rate * Time.deltaTime);
                    //b = Mathf.MoveTowards(b, 32, col_intensity_rate * Time.deltaTime);

                    //crystalTemperatureArea.GetComponent<MeshRenderer>().enabled = false;
                    foreach (GameObject temperatureObject in objectsInTempArea.Keys)
                    {
                        if (temperatureObject.GetComponent<Collider>() != null)
                        {
                            //bluelight.intensity = Mathf.MoveTowards(bluelight.intensity, light_min, intensity_rate * Time.deltaTime);
                            //redlight.intensity = Mathf.MoveTowards(redlight.intensity, light_min, intensity_rate * Time.deltaTime);
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
        }

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!objectsInTempArea.ContainsKey(other.gameObject))
        {
            if(!other.gameObject.GetComponent<MachineFXController>())
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
            if (temperatureObject.GetComponent<IConditions>() != null)
            {
                temperatureObject.GetComponent<IConditions>().AddCondition(IConditions.ConditionTypes.ConditionCold);
            }
            else if (temperatureObject.GetComponent<Collider>() != null)
            {
                temperatureObject.GetComponent<Collider>().material = icyPhysicMaterial;
                //temperatureObject.GetComponent<Collider>().material.dynamicFriction = 0.05F;
                //temperatureObject.GetComponent<Collider>().material.staticFriction = 0.05F;
                //temperatureObject.GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
            }

            //Added IConditions
  
        }
    }

}

