using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerFXController : FXController
{
    public bool isEnabled;
    public float power = 0.25f;
    public float range;

    public LineRenderer line;

    private RaycastHit hit;
    public Transform rayOrigin;



    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rayOrigin = transform.Find("RayOrigin");
        if(line == null)
            line = GetComponentInChildren<LineRenderer>();

        UpdateLine();
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnabled)
        {
            if (!line.enabled)
                line.enabled = true;

            if (line != null)
                UpdateRange();

            HitStateBased();
        }
        else
        {
            if (line.enabled)
                line.enabled = false;
        }
     
        //Debug.Log("HIT TEMP STATE BASE: " + hit.collider.gameObject.name);


        //Debug.DrawRay( rayOrigin.position, -rayOrigin.up * range);
    }

    public void Enable()
    {

    }

    public void Disable()
    {

    }

    public void UpdateLine()
    {
        if(power > 0)
        {
            Color colour = GameMaster.instance.colourPallete.Positive;
            line.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
        }
        else if (power < 0)
        {
            Color colour = GameMaster.instance.colourPallete.Negative;
            line.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
        }
        else if(power == 0)
        {
            Color colour = GameMaster.instance.colourPallete.Neutral;
            line.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
        }
    }

    public void UpdateRange()
    {
        line.SetPosition(1, Vector3.up * range);
    }

    public void HitStateBased()
    {


        if (Physics.Raycast(rayOrigin.position, -rayOrigin.up, out hit, range))
        {
            Debug.DrawRay(rayOrigin.position, -transform.up * range);

            /* ITemperature objtemp = hit.collider.GetComponentInParent<ITemperature>();

             if (objtemp != null)
             {
                 objtemp.ChangeTemperature(power * Time.deltaTime);
                 Debug.Log("OBJECT HIT: " + hit.collider.gameObject.GetComponentInParent<ITemperature>().);
                 Debug.Log("OBJECT HIT CURRENT TEMP: " + hit.collider.gameObject.GetComponentInParent<ITemperature>().CurrentTemperature);
             }*/

            CrystalBehaviour objtemp = hit.collider.GetComponentInParent<CrystalBehaviour>();

            if (objtemp != null)
            {
                objtemp.ChangeTemperature(power * Time.deltaTime);
                //Debug.Log("OBJECT HIT: " + hit.collider.gameObject.GetComponentInParent<CrystalBehaviour>().gameObject.name);
                //Debug.Log("OBJECT HIT CURRENT TEMP: " + hit.collider.gameObject.GetComponentInParent<CrystalBehaviour>().CurrentTemperature);
            }
        }

        //return false;
    }
}
