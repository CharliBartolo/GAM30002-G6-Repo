using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    public enum DoorState { None, Open, Close }

    [Header("References")]
    [SerializeField] GameObject Door = null;
    [SerializeField] TemperatureStateBase Trigger = null;
    ITemperature.tempState prevTempState;

    [Header("Settings")]
    [Range(1, 10)]
    [SerializeField] float Speed = 5;
    [Range(.5f, 4.0f)]
    [SerializeField] float Delay = 1;

    [SerializeField] float closeYPos = 0.0f;
    [SerializeField] float openYPos = 0.0f;

    [Header("Audio")]
    [SerializeField] AudioClip openSFX = null;
    [SerializeField] AudioClip closeSFX = null;

    //for later use if audio/SFX for door is wanted
    #region Private 
    private bool animating = false;
    private DoorState animatingState = DoorState.None;
    private DoorState state = DoorState.None;

    //private List<Transform> inRange = new List<Transform>();
    bool isOpen = false;

    private AudioSource source = null;
    public AudioSource Source
    {
        get
        {
            if (source == null)
            {
                source = GetComponent<AudioSource>();
                if (source == null)
                {
                    source = gameObject.AddComponent<AudioSource>();
                }
            }
            return source;
        }
        set
        {
            source = value;
        }
    }

    IEnumerator IE_StartAnimating = null, IE_Animate = null, IE_OpenDoor = null;
    #endregion 

    // Start is called before the first frame update
    private void Start()
    {
        prevTempState = Trigger.CurrentTempState;
        closeYPos = Mathf.Abs(Door.gameObject.transform.position.y);
    }

    private void Update()
    {
        // If TempState has changed
        if (Trigger.CurrentTempState != prevTempState)
        {
            prevTempState = Trigger.CurrentTempState;

            if (Trigger.CurrentTempState == ITemperature.tempState.Hot)//if cold sensor
            {
                state = DoorState.Open;
                StartAnimating();
                Debug.Log("Cold door open");
            }
            else if (Trigger.CurrentTempState == ITemperature.tempState.Cold)
            {
                
            }
            else
            {
                state = DoorState.Close;
                StartAnimating();
                Debug.Log("Door close");
            }
        }
    }

    //Check for animation
    void StartAnimating()
    {
        if (IE_StartAnimating != null)
        {
            StopCoroutine(IE_StartAnimating);
        }

        IE_StartAnimating = Begin();
        StartCoroutine(IE_StartAnimating);
    }

    //start animation
    IEnumerator Begin()
    {
        while (animating)
        {
            yield return null;
        }

        if (IE_Animate != null)
        {
            StopCoroutine(IE_Animate);
        }

        IE_Animate = Animate(state.Equals(DoorState.Open) ? openYPos : closeYPos);//passing the x position of the door
        StartCoroutine(IE_Animate);
    }

    //animation of the door
    IEnumerator Animate(float yPos)
    {
        if (Utility.Approximately(transform.localPosition.y, yPos, .001f))
        {
            yield break;
        }

        animatingState = state;

        yield return new WaitForSeconds(animatingState.Equals(DoorState.Close) ? Delay / 2 : Delay);

        if (IE_OpenDoor != null)
        {
            StopCoroutine(IE_OpenDoor);
        }

        IE_OpenDoor = Move(Door.gameObject.transform, yPos);
        StartCoroutine(IE_OpenDoor);

        PlaySound(state == DoorState.Open ? openSFX : closeSFX);//play SFX when open/close

        while (animating)
        {
            yield return null;
        }
    }

    //play SFX
    private void PlaySound(AudioClip clip)
    {
        Source.clip = clip;
        Source.Play();
    }

    //move door
    IEnumerator Move(Transform transform, float yPos)
    {
        animating = true;
        while (!Utility.Approximately(transform.localPosition.y, yPos, .001f))
        {
            float newYPos = transform.localPosition.y;
            newYPos = Mathf.Lerp(newYPos, yPos, Speed * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, newYPos, transform.localPosition.z);
            yield return null;
        }
        animating = false;
    }
}