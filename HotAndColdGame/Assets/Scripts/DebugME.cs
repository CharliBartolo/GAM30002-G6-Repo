using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugME : MonoBehaviour
{
    [Header("Transforms")]
    public Transform player;
    public UnityEngine.UI.Text temperatureField;
    public UnityEngine.UI.Text temperatureField2;
    public UnityEngine.UI.Text temperatureStateField;


    [Header("Values")]
    [SerializeField] private ITemperature whatsMyTemp;
    [SerializeField] private TemperatureStateBase whatsMyTempAgain;
    [SerializeField] private ITemperature.tempState whatsMyTempState;


    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        whatsMyTemp = player.GetComponent<ITemperature>();
        whatsMyTempAgain = player.GetComponent<TemperatureStateBase>();
        whatsMyTempState = player.GetComponent<ITemperature>().CurrentTempState;

        temperatureField.text = whatsMyTemp.CurrentTemperature.ToString();
        temperatureField2.text = whatsMyTempAgain.CurrentTemperature.ToString();
        temperatureStateField.text = whatsMyTempState.ToString();
    }
}
