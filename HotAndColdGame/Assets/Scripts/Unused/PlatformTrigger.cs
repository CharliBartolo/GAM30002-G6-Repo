using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : TemperatureStateBase
{
    //public PlatformWithTemperature platform;

    public Vector3[] points;
    public int point_number = 0;
    private Vector3 crnt_target;

    public float tolerance;
    public float speed;
    public float delay_time;

    private float delay_start;


    // Start is called before the first frame update
    void Start()
    {
       
        if (points.Length > 0)
        {
            crnt_target = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != crnt_target)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    void MovePlatform()
    {
        Vector3 heading = crnt_target - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance)
        {
            transform.position = crnt_target;
            delay_start = Time.time;
        }
    }

    void UpdateTarget()
    {
        /*if (automatic) 
        {

        }*/
        if (Time.time - delay_start > delay_time)
        {

            NextPlatform();
        }
    }

    public void NextPlatform()
    {
        point_number++;
        if (point_number >= points.Length)
        {
            point_number = 0;
        }
        crnt_target = points[point_number];
    }

}
