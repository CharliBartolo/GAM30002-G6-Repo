using UnityEngine;
using UnityEngine.UI;

public class CameraFieldOfView : MonoBehaviour
{
    public Slider mainSlider;

    void Update()
    {
        Camera.main.fieldOfView = mainSlider.value;
    }
}