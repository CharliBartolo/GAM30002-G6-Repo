using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    //public Collider Operator;
    public List<WeightedObject> WeightOperators = new List<WeightedObject>();
    public List<Rigidbody> RigidBodyOperators = new List<Rigidbody>();
    public List<WeightedObject> WeightOperating = new List<WeightedObject>();
    public List<Rigidbody> RigidBodyOperating = new List<Rigidbody>();

    public bool Pressed = false;
    public bool PressedDown = false;
    public bool MassOperated = false;
    public float OperatingWeight = 0.0f;
    [SerializeField]
    private float _currentWeight = 0.0f;

    //PushDown Effect variables
    public Transform TransformPad = null;
    public float PushdownUnit = 0.0f;
    public float PushdownSpeed = 0.0f;
    public bool InAnmimation = false;
    public float Counter = 0.0f;
    public bool InCollison = false;
    /*
    public bool OnTrigger;
    public float StartPos;
    public float EndPos;
    public float Distance;
    public float Speed;
    private float _rate;
    public float Timer = 0.0f;
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        StartPos = transform.position.y;
        Distance = -5.0f;
        Speed = 1.0f;
        _rate = 1.0f / Mathf.Abs(Distance) * Speed;
        Timer = 0.0f;
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (OnTrigger)
        {
            if (Timer < 1.0f)
            {
                Debug.Log(Timer);
                Timer += Time.deltaTime * _rate;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(StartPos, EndPos, Timer), transform.position.z);
            }
        }
        */

        if (!Pressed)
        {
            if (CurrentWeight >= OperatingWeight)
            {
                Counter = 0.0f;
                Pressed = true;              
                Debug.Log("Pressed Successful");
            }
        }
        
        else if (Pressed && PressedDown)
        {          
            if (CurrentWeight <= OperatingWeight)
            {
                Counter += 1.0f * Time.deltaTime;
                if (Counter >= 5.0f)
                {
                    if (!InAnmimation)
                    {
                        Pressed = false;
                        PressedDown = false;
                        //StartCoroutine(MoveDown(TransformPad, -PushdownUnit, PushdownSpeed));
                        MoveDown(TransformPad, -PushdownUnit, PushdownSpeed);
                        Debug.Log("Unpressed Successful");
                    }
                }      
            }
        }
        
        if (Pressed && !PressedDown)
        {
            if (!InAnmimation)
            {               
                PressedDown = true;
                //StartCoroutine(MoveDown(TransformPad, PushdownUnit, PushdownSpeed));
                MoveDown(TransformPad, PushdownUnit, PushdownSpeed);
            }          
        }
    }

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
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (!Pressed)
        {
            foreach (WeightedObject c in Operators)
            {
                if (other == c.GetCollider())
                {
                    if(c.GetWeight() >= OperatingWeight)
                    {
                        Pressed = true;
                        //MoveDown(transform, -5, 1f);
                        Debug.Log("CollisionEnter Successful");
                    }                   
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (WeightedObject c in Operators)
        {
            if (other == c.GetCollider())
            {
                Pressed = false;
                Debug.Log("CollisionExit Successful");
            }
        }
    }
    */
    IEnumerator MoveDownCoroutine(Transform thisTransform, float distance, float speed)
    {
        InAnmimation = true;
        float startPos = thisTransform.position.y;
        float endPos = startPos - distance;
        float rate = 1.0f / Mathf.Abs(startPos - endPos) * speed;
        float t = 0.0f;
        while (t < 1.0f)
        {
            //Debug.Log(t);
            t += Time.deltaTime * rate;
            thisTransform.position = new Vector3(thisTransform.position.x, Mathf.Lerp(startPos, endPos, t), thisTransform.position.z);
        }
        InAnmimation = false;
        yield return null;
    }

   private void MoveDown(Transform thisTransform, float distance, float speed)
    {
        InAnmimation = true;
        float startPos = thisTransform.position.y;
        float endPos = startPos - distance;
        float rate = 1.0f / Mathf.Abs(startPos - endPos) * speed;
        float t = 0.0f;
        while (t < 1.0f)
        {
            //Debug.Log(t);
            t += Time.deltaTime * rate;
            thisTransform.position = new Vector3(thisTransform.position.x, Mathf.Lerp(startPos, endPos, t), thisTransform.position.z);
        }
        InAnmimation = false;
    }
}
