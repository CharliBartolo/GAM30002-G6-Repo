using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicConditionsBehaviour : MonoBehaviour, IConditions
{
    [SerializeField] private List<IConditions.ConditionTypes> _activeConditions;
    Rigidbody rbComponent;
    Collider colliderComponent;
    PhysicMaterial defaultPhysicMat;
    PhysicMaterial icyPhysicMat;

    // Start is called before the first frame update
    void Start()
    {
        rbComponent = GetComponent<Rigidbody>();

        if (TryGetComponent<Collider>(out colliderComponent))
        {
            defaultPhysicMat = colliderComponent.material;
            icyPhysicMat = new PhysicMaterial ();
            icyPhysicMat.dynamicFriction = 0f;
            icyPhysicMat.staticFriction = 0f;
            icyPhysicMat.frictionCombine = PhysicMaterialCombine.Minimum;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ExecuteConditions();
    }

    public void ExecuteConditions()
    {
        // If player is hot and NOT cold
        if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold))
        {
            UpwardForce();
            ResetSlip();
        }
        // If object is cold and NOT hot
        else if (ActiveConditions.Contains(IConditions.ConditionTypes.ConditionCold) && !ActiveConditions.Contains(IConditions.ConditionTypes.ConditionHot))
        {            
            IcySlip();            
        } 
        else
        {
            ResetSlip();
        }
    }

    public void UpwardForce()
    {
        if (rbComponent != null)
        {
            rbComponent.AddForce(-Physics.gravity * Time.deltaTime * 25f, ForceMode.Acceleration);
        }
    }

    public void IcySlip()
    {
        if (colliderComponent != null)
        {
            colliderComponent.material = icyPhysicMat;            
        }
    }

    public void ResetSlip()
    {
        if (colliderComponent != null)
        {
            colliderComponent.material = defaultPhysicMat;
        }
    }

    public void AddCondition(IConditions.ConditionTypes nameOfCondition)
    {
        if (!ActiveConditions.Contains(nameOfCondition))
        {
            _activeConditions.Add(nameOfCondition);
        }        
    }

    public void RemoveCondition(IConditions.ConditionTypes nameOfCondition)
    {
        if (ActiveConditions.Contains(nameOfCondition))
        {
            _activeConditions.Remove(nameOfCondition);
        }       
    }

    public List<IConditions.ConditionTypes> ActiveConditions
    {
        get => _activeConditions;
        set
        {
            _activeConditions = value;
            //isConditionChanging = true;
        }        
    }
}
