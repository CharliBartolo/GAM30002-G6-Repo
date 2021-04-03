using UnityEngine;
using System.Collections;


//NOTE: OnTriggerEnter dependent on one object being a rigidbody
public class LightTriggers : MonoBehaviour{
    private enum LightType {Spotlight, Directional, Pointlight, Area};
    [SerializeField]
    private LightType lightType = LightType.Spotlight;  
    private Light lightComponent;

    private void Awake() 
    {
        SetupTriggerArea();
    }

    public bool CheckIfInLightArea(GameObject other)
    {      

        switch (lightType)
        {
            case LightType.Spotlight:
                // Use Vectors (Light - other) and (light - light + fwd * light range)
                //Vector3 otherRelativeToLightPos = transform.position - GetComponent<Collider>().ClosestPoint(other.transform.position);
                Vector3 otherRelativeToLightPos = transform.position - getClosestPointOnColliderRelativeToLight(other);

                //Vector3 closestRelativePoint = Physics.ClosestPoint(other.transform.position, GetComponent<Collider>(), transform.position, transform.rotation);
                Vector3 lightMaxRangePos = transform.position - (transform.position + transform.forward * lightComponent.range);                
                if (Vector3.Angle(otherRelativeToLightPos, lightMaxRangePos) > lightComponent.spotAngle / 2f)
                {
                    //Debug.Log("Not in spotlight");
                    return false;
                }
                break;            
            default:
                break;
        }        

        return CheckIfLightRayHits(other);        
    }

    void SetupTriggerArea()
    {
        lightComponent = GetComponent<Light>();

        switch (lightType)
        {
            case LightType.Spotlight:
                SetupBoxCollider();
                break;            
            default:
                break;
        }        
    }

    void OnTriggerEnter(Collider other)
    {
        //isPlayerInLight = true;

        Debug.Log("Object has entered light area");
        if (other.GetComponent<LightDetection>() != null)
        {
            other.GetComponent<LightDetection>().AddLight(this);
        }    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LightDetection>() != null)
        {
            other.GetComponent<LightDetection>().RemoveLight(this);
        }  
    }

    void SetupBoxCollider()
    {
        // Sets up a box collider that matches the bounds of the spotlight
        BoxCollider boxCol = gameObject.AddComponent<BoxCollider>();
        boxCol.isTrigger = true;
        boxCol.center = new Vector3 (0f, 0f, lightComponent.range / 2);

        // Gets length of 'cone' radius to figure out box base length
        float spotLightTan = Mathf.Tan((lightComponent.spotAngle * Mathf.Deg2Rad) / 2);
        float boxLength = Mathf.Abs(lightComponent.range * spotLightTan) * 2;
        boxCol.size = new Vector3 (boxLength, boxLength, lightComponent.range);
    }

    bool CheckIfLightRayHits(GameObject other)
    {
        RaycastHit hit;     

        //Cast line to point on other collider closest to centre ray, but equal magnitude distance as other from light        
        Vector3 pointToCastTo = getClosestPointOnColliderRelativeToLight(other);

        Debug.DrawLine(transform.position, pointToCastTo);
        if (Physics.Linecast(transform.position, pointToCastTo, out hit, ~0,  QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.name == other.name)
            {
                return true;
            }
            else 
            {
                Debug.Log(other.gameObject.name + " is in the way");
                Debug.Log("Was expecting " + hit.collider.name);
            }
        }
        else
        {
            return true;
        }

        return false;
    }

    // Returns closest point on the OTHER collider to the centre of the light beam
    Vector3 getClosestPointOnColliderRelativeToLight(GameObject otherObject)
    {
        return otherObject.GetComponent<Collider>().ClosestPoint(transform.position + transform.forward * (transform.position - otherObject.transform.position).magnitude);
    }
}
