using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    /// <summary>
    /// Pressure pad controller
    /// Uses "operator" game objects that can operate the pressure pad
    /// Pad must have a second collider where Istrigger is set to true. (collider should protude the surface of the pad.)
    /// Must add the respective "operators" to the weight/mass Operators list.
    /// MassOperated uses the rigidbody of gameobjects and Weight uses the Weightobject script.
    /// XVertical and ZVertical are for pads placed on walls VERTICALLY.
    /// </summary>
    //public Collider Operator;
    public List<WeightedObject> WeightOperators = new List<WeightedObject>();   
    public List<WeightedObject> WeightOperating = new List<WeightedObject>();
    public List<Rigidbody> RigidBodyOperating = new List<Rigidbody>();
    public List<Rigidbody> RigidBodyOperators = new List<Rigidbody>();

    public bool Pressed = false;
    public bool PressedDown = false;
    public bool MassOperated = false;
    public float OperatingWeight = 0.0f;
    public bool XVertical = false;
    public bool ZVertical = false;
    [SerializeField]
    private float _currentWeight = 0.0f;

    //PushDown Effect variables
    public float PushdownUnit = 0.0f;
    public float PushdownSpeed = 0.0f;
    public bool InAnmimation = false;
    public float Counter = 0.0f;
    public float MaxCounter = 0.0f;

    void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        //If pad is not activated (No depression yet)
        if (!Pressed)
        {
            if (CurrentWeight <= OperatingWeight)
            {
                Counter = 0.0f;
            }
            if (CurrentWeight >= OperatingWeight)
            {
                Counter += 1.0f * Time.deltaTime;
                if (Counter >= MaxCounter)
                {
                    Counter = 0.0f;
                    Pressed = true;
                    Debug.Log("Pressed Successful");
                }
            }
        }      
        //If pad is activate and depressed
        else if (Pressed && PressedDown)
        {
            if (CurrentWeight >= OperatingWeight)
            {
                Counter = 0.0f;
            }
            if (CurrentWeight <= OperatingWeight)
            {
                Counter += 1.0f * Time.deltaTime;
                if (Counter >= MaxCounter)
                {
                    Counter = 0.0f;
                    if (!InAnmimation)
                    {
                        Pressed = false;
                        PressedDown = false;
                        StartCoroutine(MoveDownCoroutine(transform, -PushdownUnit, PushdownSpeed));
                        //MoveDown(TransformPad, -PushdownUnit, PushdownSpeed);
                        Debug.Log("Unpressed Successful");
                    }
                }      
            }
        }

        //If pressed but not depressed
        if (Pressed && !PressedDown)
        {
            //Depresses pad
            if (!InAnmimation)
            {               
                PressedDown = true;
                StartCoroutine(MoveDownCoroutine(transform, PushdownUnit, PushdownSpeed));
                //MoveDown(TransformPad, PushdownUnit, PushdownSpeed);
            }          
        }
    }
    /// <summary>
    /// Gets current weight of all operators "touching" pad
    /// </summary>
    public float CurrentWeight
    {
        get
        {
            if (MassOperated)
            {
                _currentWeight = 0.0f;
                foreach (Rigidbody rb in RigidBodyOperating)
                {
                    _currentWeight += rb.mass;
                }
                return _currentWeight;
            }
            else
            {
                _currentWeight = 0.0f;
                foreach (WeightedObject wo in WeightOperating)
                {
                    _currentWeight += wo.Weight;
                }
                return _currentWeight;
            }
            
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (!Pressed)
        {
            if (MassOperated)
            {
                foreach (Rigidbody rb in RigidBodyOperators)
                {
                    if (collision.rigidbody == rb)
                    {
                        Debug.Log("CollisionEnter Successful");
                        foreach (Rigidbody rb2 in RigidBodyOperating)
                        {
                            if (rb == rb2)
                            {
                                return;
                            }
                            RigidBodyOperating.Add(rb);
                        }
                        if (WeightOperating.Count == 0)
                        {
                            RigidBodyOperating.Add(rb);
                        }
                    }
                }
            }
            else
            {
                foreach (WeightedObject wo in WeightOperators)
                {
                    if (collision.collider == wo.Collider)
                    {
                        Debug.Log("CollisionEnter Successful");
                        
                    wo.Touching = true;
                        foreach (WeightedObject wo2 in WeightOperating)
                        {
                            if (wo2 == wo)
                            {
                                return;
                            }
                            WeightOperating.Add(wo);
                        }
                        if (WeightOperating.Count == 0)
                        {
                            WeightOperating.Add(wo);
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {  
        if (MassOperated)
        {
            foreach (Rigidbody rb in RigidBodyOperators)
            {
                if (collision.rigidbody == rb)
                {
                    RigidBodyOperating.Remove(rb);
                    Debug.Log("CollisionExit Successful");
                }
            }
        }
        else
        {
            foreach (WeightedObject wo in WeightOperators)
            {
                if (collision.collider == wo.Collider)
                {
                    wo.Touching = false;
                    WeightOperating.Remove(wo);
                    Debug.Log("CollisionExit Successful");
                }
            }
        }
    }
    */

    //When operators "touch" pressure pad
    private void OnTriggerEnter(Collider other)
    {
        //Rigidbody vs WeightedObject check
        if (MassOperated)
        {
            //Adds operators to RigidBodyOperating
            foreach (Rigidbody rb in RigidBodyOperators)
            {
                if (other.attachedRigidbody == rb)
                {
                    Debug.Log("CollisionEnter Successful");
                    foreach (Rigidbody rb2 in RigidBodyOperating)
                    {
                        if (rb == rb2)
                        {
                            return;
                        }
                        RigidBodyOperating.Add(rb);
                    }
                    if (WeightOperating.Count == 0)
                    {
                        RigidBodyOperating.Add(rb);
                    }
                }
            }
        }
        else
        {
            //Adds operators to WeightedOperating
            foreach (WeightedObject wo in WeightOperators)
            {
                if (other == wo.Collider)
                {
                    Debug.Log("CollisionEnter Successful");

                    wo.Touching = true;
                    foreach (WeightedObject wo2 in WeightOperating)
                    {
                        if (wo2 == wo)
                        {
                            return;
                        }
                        WeightOperating.Add(wo);
                    }
                    if (WeightOperating.Count == 0)
                    {
                        WeightOperating.Add(wo);
                    }
                }
            }
        }
        
    }

    //When operators leave pressure pad
    private void OnTriggerExit(Collider other)
    {
        //Rigidbody vs WeightedObject check
        if (MassOperated)
        {
            //Removes operators from RigidBodyOperating 
            foreach (Rigidbody rb in RigidBodyOperators)
            {
                if (other.attachedRigidbody == rb)
                {
                    RigidBodyOperating.Remove(rb);
                    Debug.Log("CollisionExit Successful");
                }
            }
        }
        else
        {
            //Removes operators from WeightedOperating 
            foreach (WeightedObject wo in WeightOperators)
            {
                if (other == wo.Collider)
                {
                    wo.Touching = false;
                    WeightOperating.Remove(wo);
                    Debug.Log("CollisionExit Successful");
                }
            }
        }
    }
    
    /// <summary>
    /// Function to depress the pad into the wall/floor below/behind it.
    /// </summary>
    /// <param name="thisTransform">The transform component of pad</param>
    /// <param name="distance">How far the pad will travel when pressed</param>
    /// <param name="speed">How fast the pad will travel.</param>
    /// <returns></returns>
    IEnumerator MoveDownCoroutine(Transform thisTransform, float distance, float speed)
    {
        InAnmimation = true;
        float startPos = 0.0f;

        if (ZVertical)
        {
            startPos = transform.position.z;
        }
        else if (XVertical)
        {
            startPos = transform.position.x;
        }
        else
        {
            startPos = transform.position.y;
        }

        float endPos = startPos - distance;
        float rate = 1.0f / Mathf.Abs(startPos - endPos) * speed;
        float t = 0.0f;

        while (t < 1.0f)
        {
            //Debug.Log(t);
            t += Time.deltaTime * rate;
            if (ZVertical)
            {
                thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, Mathf.Lerp(startPos, endPos, t));
            }
            else if (XVertical)
            {
                thisTransform.position = new Vector3(Mathf.Lerp(startPos, endPos, t), thisTransform.position.y, thisTransform.position.z);
            }
            else
            {
                thisTransform.position = new Vector3(thisTransform.position.x, Mathf.Lerp(startPos, endPos, t), thisTransform.position.z);
            }
            yield return null;
        }
        InAnmimation = false;
        
    }
    /*
   private void MoveDown(Transform thisTransform, float distance, float speed)
    {
        InAnmimation = true;
        float startPos = 0.0f;
        

        if (ZVertical)
        {
            startPos = transform.position.z;
        }
        else if (XVertical)
        {
            startPos = transform.position.x;
        }
        else
        {
            startPos = transform.position.y;
        }

        float endPos = startPos - distance;
        float rate = 1.0f / Mathf.Abs(startPos - endPos) * speed;
        float t = 0.0f;

        while (t < 1.0f)
        {
            //Debug.Log(t);
            t += Time.deltaTime * rate;
            if (ZVertical)
            {
                thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, Mathf.Lerp(startPos, endPos, t));
            }
            else if (XVertical)
            {
                thisTransform.position = new Vector3(Mathf.Lerp(startPos, endPos, t), thisTransform.position.y, thisTransform.position.z);
            }
            else
            {
                thisTransform.position = new Vector3(thisTransform.position.x, Mathf.Lerp(startPos, endPos, t), thisTransform.position.z);
            }        
        }
        InAnmimation = false;
    }
    */
}
