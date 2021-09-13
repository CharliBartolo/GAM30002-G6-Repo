using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundControl : MonoBehaviour
{
    public float timeBetweenFootsteps = 1f;
    private float currentTimeBetweenFootsteps = 0f;

    [SerializeField] private List<AudioClip> footstepSounds = new List<AudioClip>(); 
    [SerializeField] private List<AudioClip> landingSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> clamberingSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> raygunSounds = new List<AudioClip>();

    private AudioSource playerAudio;
    public AudioSource raygunAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();

        raygunAudio = GetComponent<GunFXController>().gun_obj.GetComponent<AudioSource>();
    }

    // Sound Functions Below
    public void CalculateTimeToFootstep(Vector3 horizVelocity, bool isGrounded)
    {
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
            //GameMaster.instance.audioManager.Play("Footstep");
            PlayFootStepAudio(isGrounded);
            currentTimeBetweenFootsteps = timeBetweenFootsteps;
        }
    }

    private void PlayFootStepAudio(bool isGrounded)
    {
        /*
        if(!isGrounded)
        {
            return;
        }
        
        if (footstepSounds.Count > 0)
        {
            int n = Random.Range(1, footstepSounds.Count);
            playerAudio.clip = footstepSounds[n];
            playerAudio.pitch = Random.Range(0.8f, 1.2f);
            playerAudio.PlayOneShot(playerAudio.clip);
            footstepSounds[n] = footstepSounds[0];
            footstepSounds[0] = playerAudio.clip;
        }    
        */
        SelectAndPlayRandomClip(footstepSounds);    
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
        SelectAndPlayRandomClip(landingSounds);
    }

    public void PlayClamberingAudio()
    {
        SelectAndPlayRandomClip(clamberingSounds);
    }

    public void PlayDeathAudio()
    {

    }

    private void SelectAndPlayRandomClip(List<AudioClip> clipsToSelectFrom)
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
            playerAudio.pitch = Random.Range(0.8f, 1.2f);
            playerAudio.PlayOneShot(playerAudio.clip);
            clipsToSelectFrom[n] = clipsToSelectFrom[0];
            clipsToSelectFrom[0] = playerAudio.clip;
        }     
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
