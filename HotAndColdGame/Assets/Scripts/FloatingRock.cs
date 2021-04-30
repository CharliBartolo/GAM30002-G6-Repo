using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRock : MonoBehaviour
{
    //object should be placed about 5 units above the floor
    public TemperatureStateBase TSB;//Requires the TemperatureStateBase script
    public Rigidbody RB;//The rigidbody of the current object (floating rock)
    public SpringJoint SJ;//The springjoint component
    public float MoveForce = 10f;//The amount of force used to keep the spring moving. (Trying to keep the bobbing effect instead the spring coming to a halt.)

    [SerializeField]
    private ITemperature.tempState temp;//Returns the temp of rock
    [SerializeField]
    private float coldSpring = 0f;//Controls how much spring (bobbing-ness) the rock has when cold
    [SerializeField]
    private float neutralSpring = 5f;//Controls how much spring (bobbing-ness) the rock has when neutral
    [SerializeField]
    private float hotSpring = 10f;//Controls how much spring (bobbing-ness) the rock has when hot

    // Start is called before the first frame update
    void Start()
    {
        //default values (feel free to change)
        //MoveForce = 10f;
        //coldSpring = 0f;
        //neutralSpring = 5f;
        //hotSpring = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        temp = TSB.CurrentTempState; //Gets the temp

        //passes temp through switch
        switch (temp)
        {
            //changes the spring of spring joint accordiningly
            //cold - does not bob
            case ITemperature.tempState.Cold:
                SJ.spring = coldSpring;
                break;

            //neutral - bobs 
            case ITemperature.tempState.Neutral:
                SJ.spring = neutralSpring;
                RB.AddForce(transform.up * -MoveForce);
                break;

            //hot - bobs violently
            case ITemperature.tempState.Hot:
                SJ.spring = hotSpring;
                RB.AddForce(transform.up * -MoveForce);
                break;
        }
    }
}
