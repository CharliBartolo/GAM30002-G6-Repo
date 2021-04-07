using UnityEngine;
using System.Collections;


//NOTE: OnTriggerEnter dependent on one object being a rigidbody
public class LightTriggers : MonoBehaviour{
    // LightTriggers work in three stages:
    // 1. Detect if an object is in the potential light area via box collider triggers.
    // 2. If the object is in the collider, check its positional angle to see if its in the light.
    // 3. If it IS in the angle, cast a ray to the other object to see if it is unobstructed. Return lit if ray reaches object or hits nothing.

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
                return SpotLightDetection(other);                
            default:
                return CheckIfLightRayHits(other, other.transform.position);
        }             
    }

    bool SpotLightDetection(GameObject other)
    {
        Vector3 lightMaxRangePos = transform.position - (transform.position + transform.forward * lightComponent.range);
        Vector3[] positionsToCheck;

        if (other.GetComponent<LightDetection>().useComplexLightDetection)
        {
            Mesh mesh = other.GetComponent<MeshFilter>().mesh;
            positionsToCheck = new Vector3[mesh.vertices.Length + 2];
            //positionsToCheck = mesh.vertices;
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                positionsToCheck[i] = other.transform.TransformPoint(mesh.vertices[i]);
            }
            positionsToCheck[positionsToCheck.Length - 1] = other.transform.position;
            positionsToCheck[positionsToCheck.Length - 2] = getClosestPointOnColliderRelativeToLight(other);
        }
        else
        {
            positionsToCheck = new Vector3[2];            
            positionsToCheck[0] = other.transform.position;
            positionsToCheck[1] = getClosestPointOnColliderRelativeToLight(other);
        }    

        // If any position is successfully hit, return true, else return false.
        for (int i = 0; i < positionsToCheck.Length; i++)
        {
            if (CheckIfWithinAngle(transform.position - (positionsToCheck[i]), lightMaxRangePos))
            {
                //Debug.Log("At least one vertice is in the angle");
                if (CheckIfLightRayHits(other, positionsToCheck[i]))
                {
                    return true;
                }
            }                
        }                          
                
        return false;            
    }

    bool CheckIfLightRayHits(GameObject other, Vector3 pointToCastTo)
    {
        RaycastHit hit;     

        //Cast line to point on other collider closest to centre ray, but equal magnitude distance as other from light        
        //Vector3 pointToCastTo = getClosestPointOnColliderRelativeToLight(other);

        Debug.DrawLine(transform.position, pointToCastTo);
        if (Physics.Linecast(transform.position, pointToCastTo, out hit, ~0,  QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.name == other.name)
            {
                return true;
            }
            else 
            {
                //Debug.Log(other.gameObject.name + " is in the way");
                //Debug.Log("Was expecting " + hit.collider.name);
            }
        }
        else
        {
            return true;
        }

        return false;
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

    Vector3 getClosestPointOnColliderRelativeToLight(GameObject otherObject)
    {
        return otherObject.GetComponent<Collider>().ClosestPoint(transform.position + transform.forward * (transform.position - otherObject.transform.position).magnitude);
    }

    bool CheckIfWithinAngle(Vector3 startPos, Vector3 endPos)
    {
        if (Vector3.Angle(startPos, endPos) > lightComponent.spotAngle / 2f)
        {
            //Debug.Log("Not in spotlight");
            return false;
        }
        else
            return true;
    }
}
