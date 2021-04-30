using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class RayCastShootComplete : MonoBehaviour {

	public float tempchange = 1f;
	public float weaponRange = 50f;
	public float hitForce = 5f;
	public Transform gunEnd;
	private Camera fpsCam;
    public GameObject spherecollider;
    public GameObject particleAtEnd_ice;
    public GameObject particleAtEnd_fire;
    private bool triggerHeld = false;

    //Laser Properties
    private LineRenderer laserLine;
    public Material hotMaterial;
    public Material coldMaterial;
    public bool cold = true;
    //public Color col = Color.blue;

	void Start () 
	{
		laserLine = GetComponent<LineRenderer>();
		fpsCam = GetComponentInParent<Camera>();
	}

    public void SwapBeam(InputAction.CallbackContext context)
    {
        if (context.performed)
            cold = !cold;
    }

    public void FireBeam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            triggerHeld = true;
        }
        else if (context.canceled)
        {
            triggerHeld = false;
        }
    }   

	

	void LateUpdate () 
	{
        // Change Temperature States
        /*        
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            cold = true;
            print(cold);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            cold = false;
            print(cold);
        }
        */

        //if (Input.GetButton("Fire1"))
        if (triggerHeld) 
		{

            laserLine.enabled = true;

            if (cold == true)
            {
                //col = Color.blue;
                laserLine.material = coldMaterial;
                tempchange = -60f * Time.deltaTime;
            }

            if (cold == false)
            {
                //col = Color.red;
                laserLine.material = hotMaterial;
                tempchange = 60f * Time.deltaTime;
            }

            //laserLine.startColor = col;
            //laserLine.endColor = col;

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
			laserLine.SetPosition (0, gunEnd.position);

			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				laserLine.SetPosition (1, hit.point);
				ITemperature objtemp = hit.collider.GetComponent<ITemperature>();

				if (objtemp != null)
				{
					objtemp.ChangeTemperature(tempchange);
				}

				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce (-hit.normal * 20);
                }

                
                //Instantiate(spherecollider, hit.point, Quaternion.identity);
                //spherecollider.GetComponent<ShootEnd>().temperature = tempchange;
                if (cold == true)
                {
                    particleAtEnd_ice.SetActive(true);
                    particleAtEnd_fire.SetActive(false);
                    particleAtEnd_ice.transform.position = hit.point;
                }
                if (cold == false)
                {
                    particleAtEnd_fire.SetActive(true);
                    particleAtEnd_ice.SetActive(false);
                    particleAtEnd_fire.transform.position = hit.point;
                }
                

            }
			else
			{
                laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
			}
		}
        else
        {
            laserLine.enabled = false;
            particleAtEnd_ice.SetActive(false);
            particleAtEnd_fire.SetActive(false);
        }        
	}
}