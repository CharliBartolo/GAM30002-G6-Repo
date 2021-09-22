using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundControl : MonoBehaviour
{
    public float timeBetweenFootsteps = 1f;
    public float maxSpeedForSlidePitch = 20f;
    private float currentTimeBetweenFootsteps = 0f;
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
        playerAudio = GetComponent<AudioSource>();

        raygunAudio = GetComponent<GunFXController>().gun_obj.GetComponent<AudioSource>();
        activeConditions = new List<IConditions.ConditionTypes>();
    }

    public void UpdateActiveConditions(List<IConditions.ConditionTypes> updatedCondList)
    {
        activeConditions = updatedCondList;
    }

    // Sound Functions Below
    public void CalculateTimeToFootstep(Vector3 horizVelocity, bool isGrounded)
    {
        if (slidingAudio.isPlaying)
        {
            slidingAudio.Stop();
        }

        //Debug.Log(currentTimeBetweenFootsteps);
        if (horizVelocity.magnitude > 0f && horizVelocity.magnitude < 10f && isGrounded)
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
                n = Random.Range(1, clipsToSelectFrom.Count);
            }
            
            playerAudio.clip = clipsToSelectFrom[n];          
            clipsToSelectFrom[n] = clipsToSelectFrom[0];
            clipsToSelectFrom[0] = playerAudio.clip;
        }     
    }

    private void RandomisePitch(List<AudioClip> clipsToSelectFrom)
    {
        playerAudio.pitch = Random.Range(0.8f, 1.2f);
    }
    

    private void PlaySelectedClipOnce(List<AudioClip> clipsToSelectFrom)
    {
        playerAudio.PlayOneShot(playerAudio.clip);
    }

    /*
    private void ProgressStepCycle(float speed) //input speed
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (Input.x != 0 || Input.y != 0))
            {
                StepCycle += (CharacterController.velocity.magnitude + (speed*(IsWalking ? 1f : RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(StepCycle > NextStep))
            {
                return;
            }

            NextStep = StepCycle + StepInterval;

            PlayFootStepAudio();
        }
    */
}
