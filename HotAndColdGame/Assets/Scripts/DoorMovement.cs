using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : TemperatureStateBase
{
    public enum DoorState { None, Open, Close }

    [Header("References")]
    [SerializeField] GameObject Door = null;
    [SerializeField] TemperatureStateBase Trigger = null;

    [Header("Settings")]
    [Range(1, 10)]
    [SerializeField] float Speed = 5;
    [Range(.5f, 4.0f)]
    [SerializeField] float Delay = 1;

    [SerializeField] float closeXPos = 0.0f;
    [SerializeField] float openXPos = 0.0f;
    /*[SerializeField] float closeYPos = 0.0f;
    [SerializeField] float openYPos = 0.0f;*/

    [Header("Audio")]
    [SerializeField] AudioClip openSFX = null;
    [SerializeField] AudioClip closeSFX = null;

    //for later use if audio/SFX for door is wanted
    #region Private 
    private bool animating = false;
    private DoorState animatingState = DoorState.None;
    private DoorState state = DoorState.None;

    private List<Transform> inRange = new List<Transform>();

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
    protected override void Start()
    {
        if ((Trigger == null) && (GetComponent<TemperatureStateBase>() != null))
        {
            Trigger = GetComponent<TemperatureStateBase>();
        }
        else
        {
            Debug.LogWarning("Missing Sensor component. Please add one");
        }
        if ((Door == null) && (GetComponent<GameObject>() != null))
        {
            Door = GetComponent<GameObject>();
        }
        else
        {
            Debug.LogWarning("Missing Door component. Please add one");
        }
        closeXPos = Mathf.Abs(Door.gameObject.transform.position.x);
        //closeYPos = Mathf.Abs(Door.transform.position.y);
    }

    //when hit with cold/hot beam
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Trigger.CurrentTempState == TemperatureStateBase.TempState.Cold)//if cold sensor
        {
            Debug.Log("it works");
            return;
        }
        inRange.Add(other.transform);

        state = DoorState.Open;
        StartAnimating();
    }

    //after hit with cold/hot beam
    protected virtual void OnTriggerExit(Collider other)
    {
        if (Trigger.CurrentTempState == TemperatureStateBase.TempState.Cold)
        {
            return;
        }
        inRange.Remove(other.transform);

        if (inRange.Count <= 0)
        {
            state = DoorState.Close;
            StartAnimating();
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

        IE_Animate = Animate(state.Equals(DoorState.Open) ? openXPos : closeXPos);//passing the x position of the door
        StartCoroutine(IE_Animate);
    }

    //animation of the door
    IEnumerator Animate(float xPos)
    {
        if (Utility.Approximately(transform.localPosition.x, xPos, .001f))
        {
            yield break;
        }

        animatingState = state;

        yield return new WaitForSeconds(animatingState.Equals(DoorState.Close) ? Delay / 2 : Delay);

        if (IE_OpenDoor != null)
        {
            StopCoroutine(IE_OpenDoor);
        }

        IE_OpenDoor = Move(Door.gameObject.transform, xPos);
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
    IEnumerator Move(Transform transform, float xPos)
    {
        animating = true;
        while (!Utility.Approximately(transform.localPosition.x, xPos, .001f))
        {
            float newXPos = transform.localPosition.x;
            newXPos = Mathf.Lerp(newXPos, xPos, Speed * Time.deltaTime);
            transform.localPosition = new Vector3(newXPos, transform.localPosition.y, transform.localPosition.z);
            yield return null;
        }
        animating = false;
    }
}