using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class RayCastShootComplete : MonoBehaviour {
    [HideInInspector]
    public enum gunUpgrade {None, One, Two}

	public float tempchange = 1f;
	public float weaponRange = 50f;
	public float hitForce = 5f;

    public gunUpgrade gunUpgradeState = gunUpgrade.Two; 
	public Transform gunEnd;
	public Camera fpsCam;
    public AudioManager audioManager;
    public GameObject spherecollider;
    public GameObject particleAtEnd_ice;
    public GameObject particleAtEnd_fire;

    //Laser Properties
    public LineRenderer laserLine;
    public LineRenderer lightning;
    public Material hotMaterial;
    public Material coldMaterial;
    public bool cold = true;
    
    // raycast spread
    public float spread = 0.4f;

    public bool CanShoot { get; set;}
    public bool CanSwap {get; set;}
    public bool TriggerHeld { get; set;}
    public bool ModeSwitched { get; set;}

	void Start () 
	{
		laserLine = GetComponent<LineRenderer>();
		fpsCam = GetComponentInParent<Camera>();
        UpdateGunState();

        Color colour = GameMaster.instance.colourPallete.Neutral;
        laserLine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
        laserLine.SetPosition(0, gunEnd.position);
        //lightning.startColor = colour;
        //lightning.endColor = colour;
    }

    public void UpdateGunState()
    {
        CanShoot = (gunUpgradeState == gunUpgrade.One || gunUpgradeState == gunUpgrade.Two);
        CanSwap = (gunUpgradeState == gunUpgrade.Two);
    }

    public void SwapBeam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gunUpgradeState == gunUpgrade.Two)
            {
                ModeSwitched = true;
                ChangeMode();
            }
            else
            {
                // Do weird animation stuff to indicate gun can't swap
                ModeSwitched = true;
                Debug.Log("Can't swap, gun is not sufficiently upgraded!");
            }            
        } 
    }

    public void FireBeam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TriggerHeld = true;
        }
        else if (context.canceled)
        {
            TriggerHeld = false;
        }
    }   

    
    public void SetGunUpgradeState(int stateToSet)
    {
        gunUpgradeState = (gunUpgrade)stateToSet;
        UpdateGunState();
    }

    public void UpgradeGun()
    {
        gunUpgradeState = (gunUpgrade)(gunUpgradeState + 1);
        UpdateGunState();
    }

    public void ChangeMode()
    {
        cold = !cold;
    }

    void SetMOdeSwitchFalse()
    {
       
        ModeSwitched = false;
    }

    private void Update()
    {
        if (ModeSwitched)
        {
            Debug.Log("MODE SWITCHED");
            Invoke(nameof(SetMOdeSwitchFalse), Time.deltaTime);
        }
    }

    void LateUpdate () 
	{

        if (TriggerHeld && CanShoot && !GetComponentInParent<GunFXController>().inspectingWeapon && GetComponentInParent<GunFXController>().equipped && !GameObject.Find("UI").GetComponent<PauseController>().IsPaused)
		{
            
            laserLine.enabled = true;
            lightning.enabled = true;

            if (cold == true)
            {
                //col = Color.blue;
                Color colour = GameMaster.instance.colourPallete.Negative;
                laserLine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
                laserLine.material = coldMaterial;
                lightning.startColor = colour;
                lightning.endColor = colour;
                tempchange = -60f * Time.deltaTime;
            }

            if (cold == false)
            {
                //col = Color.red;
                Color colour = GameMaster.instance.colourPallete.Positive;
                laserLine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
                laserLine.material = hotMaterial;
                lightning.startColor = colour;
                lightning.endColor = colour;
                tempchange = 60f * Time.deltaTime;
            }

            // get camera centre position
            Vector3 camCentre = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // set laser line position
            laserLine.SetPosition (0, gunEnd.position);
            laserLine.SetPosition(1, camCentre + fpsCam.transform.forward * 1);

            // set lightning position
            if (lightning.enabled)
            {
                lightning.SetPosition(1, camCentre + (fpsCam.transform.forward * 2));
            }

            // create array list for hits
            ArrayList hits = new ArrayList();

            // raycasts
            Vector3 rayOrigin = camCentre;
            Vector3 rayLeft = camCentre + (-fpsCam.transform.right * spread);
            Vector3 rayRight = camCentre + (fpsCam.transform.right * spread);
            Vector3 rayUp = camCentre + (fpsCam.transform.up * spread);
            Vector3 rayDown = camCentre + (-fpsCam.transform.up * spread);

            // raycast centre
            ITemperature objtemp1 = RaycastHitTempObject(rayOrigin);
            // ray horizontal left;
            ITemperature objtemp2 = RaycastHitTempObject(rayLeft);
            // ray horizontal right;
            ITemperature objtemp3 = RaycastHitTempObject(rayRight);
            // ray vertical up;
            ITemperature objtemp4 = RaycastHitTempObject(rayUp);
            // ray vertical down;
            ITemperature objtemp5 = RaycastHitTempObject(rayDown);

            
            // add temp objects to hit list, ignoring already added objkects
            if (objtemp1 != null)
            {
                if (!hits.Contains(objtemp1))
                    hits.Add(objtemp1);
            }

            if (objtemp2 != null)
            {
                if (!hits.Contains(objtemp2))
                    hits.Add(objtemp2);
            }

            if (objtemp3 != null)
            {
                if (!hits.Contains(objtemp3))
                    hits.Add(objtemp3);
            }

            if (objtemp4 != null)
            {
                if (!hits.Contains(objtemp4))
                    hits.Add(objtemp4);
            }

            if (objtemp5 != null)
            {
                if (!hits.Contains(objtemp5))
                    hits.Add(objtemp5);
            }

            // after listing all hit temp objects, run the temp change for each
            foreach (ITemperature item in hits)
            {
                item.ChangeTemperature(tempchange);
            }
        }
        else
        {
            laserLine.enabled = false;
            lightning.enabled = false;
        }        
	}

    public ITemperature RaycastHitTempObject(Vector3 ray_origin)
    {
        if (Physics.Raycast(ray_origin, fpsCam.transform.forward, out RaycastHit hit, weaponRange) && lightning != null)
        {
            ITemperature objtemp = hit.collider.GetComponentInParent<ITemperature>();

            if (objtemp != null)
            {
                GetComponentInParent<ReticleFXController>().objHit = hit.collider.gameObject;
                return objtemp;
            }
            else
            {
                GetComponentInParent<ReticleFXController>().objHit = null;
                return null;
            }

        }

        return null;
    }
}