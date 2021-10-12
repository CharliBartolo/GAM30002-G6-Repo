using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSoundControl : MonoBehaviour
{
    public float timeBetweenFootsteps = 1f;
    public float maxSpeedForSlidePitch = 20f;
    private float currentTimeBetweenFootsteps = 0f;

    [SerializeField] private string currentLayer;
    [SerializeField] private FootstepSurfaceMaps[] footstepSurfaceMaps;
    private List<IConditions.ConditionTypes> activeConditions;

    [SerializeField] private List<AudioClip> footstepSounds = new List<AudioClip>(); 
    [SerializeField] private List<AudioClip> slideSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> landingSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> clamberingSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> raygunSounds = new List<AudioClip>();

    private AudioSource playerAudio;
    public AudioSource raygunAudio;
    public AudioSource slidingAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = Camera.main.transform.Find("AudioPosition").GetComponent<AudioSource>();

        raygunAudio = GetComponent<GunFXController>().gun_obj.GetComponent<AudioSource>();
        activeConditions = new List<IConditions.ConditionTypes>();

    }

    public void UpdateActiveConditions(List<IConditions.ConditionTypes> updatedCondList)
    {
        activeConditions = updatedCondList;
    }

    // Sound Functions Below
    public void CalculateTimeToFootstep(Vector3 horizVelocity, bool isGrounded, float coyoteTimer)
    {
        if (slidingAudio.isPlaying)
        {
            slidingAudio.Stop();
        }

        //Debug.Log(currentTimeBetweenFootsteps);
        if (!isGrounded && coyoteTimer > 0f)
        {
            // Do nothing
        }
        else if (horizVelocity.magnitude > 0f && horizVelocity.magnitude < 10f && isGrounded)
        {
            currentTimeBetweenFootsteps -= 5f * horizVelocity.magnitude * Time.deltaTime;
        }   
        else if (horizVelocity.magnitude > 10f && isGrounded)
        {
            currentTimeBetweenFootsteps -= 5f * horizVelocity.magnitude * 0.5f * Time.deltaTime;
        }
        else
        {
            currentTimeBetweenFootsteps = 0.01f;
        }

        if (currentTimeBetweenFootsteps <= 0f)
        {  
            CheckSurface();
            //if (activeConditions.Contains(IConditions.ConditionTypes.ConditionCold))
            //{
                //CalculateSlide(horizVelocity, isGrounded);
            //}
            //else
            //{
                PlayFootStepAudio();
            //}
            
            currentTimeBetweenFootsteps = timeBetweenFootsteps;                   
        }
    }

    public void CalculateSlide(Vector3 horizVelocity, bool isGrounded)
    {
        float velocityTo01 = horizVelocity.magnitude / maxSpeedForSlidePitch;
        float pitchToSet = Mathf.Lerp(1f, 1.1f, velocityTo01);   
        float volumeToSet = Mathf.Lerp(0.2f, 1f, velocityTo01);     

        //Debug.Log("Horiz Velocity Magnitude (CalcSlide) = " + horizVelocity);

        if (horizVelocity.magnitude > 0.05f && isGrounded)
        {
            if (!slidingAudio.isPlaying && slideSounds.Count > 0)
            {
                slidingAudio.clip = slideSounds[0];
                slidingAudio.loop = true;                
                PlaySlideAudio(pitchToSet);
            }     

            slidingAudio.pitch = pitchToSet;
            slidingAudio.volume = volumeToSet;           
        } 
        else
        {
            slidingAudio.volume = Mathf.Lerp(0, slidingAudio.volume, Time.deltaTime);
        }       

        //if (slidingAudio.isPlaying && slidingAudio.clip == slideSounds[0])
        //{
            
        //}
    }

    private void PlayFootStepAudio()
    {
        SelectRandomClip(footstepSounds);
        RandomisePitch(footstepSounds);
        PlaySelectedClipOnce(footstepSounds);    
    }

    private void PlaySlideAudio(float pitchValue)
    {
        //SelectRandomClip(slideSounds);
        slidingAudio.pitch = pitchValue;     
          
        slidingAudio.Play();
    }

    public void PlayRaygunAudio(int state, bool once)
    {
        raygunAudio.clip = raygunSounds[state];
        if(once)
            raygunAudio.PlayOneShot(raygunAudio.clip);
        else
            raygunAudio.Play();
    }

    public void PlayLandingAudio()
    {
        SelectRandomClip(landingSounds);
        RandomisePitch(landingSounds);
        PlaySelectedClipOnce(landingSounds);
    }

    public void PlayClamberingAudio()
    {
        SelectRandomClip(clamberingSounds);
        RandomisePitch(clamberingSounds);
        PlaySelectedClipOnce(clamberingSounds);
    }

    public void PlayDeathAudio()
    {

    }

    private void SelectRandomClip(List<AudioClip> clipsToSelectFrom)
    {
        if (clipsToSelectFrom.Count > 0)
        {
            int n;
            if (clipsToSelectFrom.Count == 1)
            {
                n = 0;
            }
            else
            {
                n = UnityEngine.Random.Range(1, clipsToSelectFrom.Count);
            }
            
            playerAudio.clip = clipsToSelectFrom[n];          
            clipsToSelectFrom[n] = clipsToSelectFrom[0];
            clipsToSelectFrom[0] = playerAudio.clip;
        }     
    }

    private void RandomisePitch(List<AudioClip> clipsToSelectFrom)
    {
        playerAudio.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
    }
    

    private void PlaySelectedClipOnce(List<AudioClip> clipsToSelectFrom)
    {
        playerAudio.PlayOneShot(playerAudio.clip);
    }

    public string GetCurrentSurface()
    {
        string materialName = "";
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5))
        {
            if (hit.collider != null) //&& hit.collider.CompareTag("Surface"))
            {
                if(hit.collider.TryGetComponent<Renderer>(out Renderer rendererComponent))
                {
                    materialName = rendererComponent.material.name;
                    print(materialName);
                }
                else if (hit.collider.TryGetComponent<Terrain>(out Terrain terrainComponent))
                {
                    materialName = terrainComponent.name;
                    print(materialName);
                }
               
            }
        }
        return materialName;
    }

    public void CheckSurface()
    {
        currentLayer = GetCurrentSurface();

        if (footstepSurfaceMaps.Length < 1)
            return;

        foreach (FootstepSurfaceMaps footstepMap in footstepSurfaceMaps)
        {
            foreach (string surface in footstepMap.surfaces)
            {
                Debug.Log(surface);
                //if (currentLayer)
                //if(currentSurfaceType == surface)
                if (CustomStartsWith(currentLayer, surface))
                {
                    SwapFootsteps(footstepMap.footstepCollection);
                    return;
                }
            }
        }

        SwapFootsteps(footstepSurfaceMaps[0].footstepCollection);
    }

    public void SwapFootsteps(FootstepCollection footstepCollection)
    {
        footstepSounds.Clear();
        for(int i = 0; i < footstepCollection.footstepSounds.Count; i++)
        {
            footstepSounds.Add(footstepCollection.footstepSounds[i]);
        }  

        //jump
        //landing
        //clambering
    }

    public bool CustomStartsWith(string a, string b)
    {
        int aLen = a.Length;
        int bLen = b.Length;
    
        int ap = 0; int bp = 0;
    
        while (ap < aLen && bp < bLen && a [ap] == b [bp])
        {
            ap++;
            bp++;
        }
    
        return (bp == bLen);
    }
}

[Serializable]
public class FootstepSurfaceMaps
{
    [SerializeField] public FootstepCollection footstepCollection;
    [SerializeField] public string[] surfaces;
}