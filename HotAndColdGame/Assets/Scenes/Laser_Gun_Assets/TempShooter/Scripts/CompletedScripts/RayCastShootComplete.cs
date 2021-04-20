using UnityEngine;
using System.Collections;

public class RayCastShootComplete : MonoBehaviour {

	public int tempchange = 0;
	public float weaponRange = 50f;
	public float hitForce = 5f;
	public Transform gunEnd;
	private Camera fpsCam;
    public GameObject spherecollider;
    public GameObject particleAtEnd_ice;
    public GameObject particleAtEnd_fire;

    //Laser Properties
    private LineRenderer laserLine;
    public bool cold = true;
    public Color col = Color.blue;

	void Start () 
	{
		laserLine = GetComponent<LineRenderer>();
		fpsCam = GetComponentInParent<Camera>();
	}
	

	void Update () 
	{
        // Change Temperature States
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

            if (Input.GetButton("Fire1")) 
		{

            laserLine.enabled = true;

            if (cold == true)
            {
                col = Color.blue;
                tempchange = 10;
            }

            if (cold == false)
            {
                col = Color.red;
                tempchange = -10;
            }

            laserLine.startColor = col;
            laserLine.endColor = col;

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
			laserLine.SetPosition (0, gunEnd.position);

			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				laserLine.SetPosition (1, hit.point);
				ShootableBox objtemp = hit.collider.GetComponent<ShootableBox>();

				if (objtemp != null)
				{
					objtemp.Temperature (tempchange);
				}

				if (hit.rigidbody != null)
				{
					hit.rigidbody.AddForce (-hit.normal * 20);
                }

                Instantiate(spherecollider, hit.point, Quaternion.identity);
                spherecollider.GetComponent<ShootEnd>().temperature = tempchange;
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