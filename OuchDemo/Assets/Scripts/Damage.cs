using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public LightDetection LD;
    public Transform player;
    private float DAMAGETIMER = 10.0f;
    private Vector3 _reset_position;
    private float _timer = 0.0f;
    private float _seconds;

    // Start is called before the first frame update
    void Start()
    {
         _reset_position = player.position;
         
    }

    // Update is called once per frame
    void Update()
    {
        _timer = 0.0f;

        while (LD.isLit == true)
        {
            _timer += Time.deltaTime;
            //Debug.Log(timer);
            _seconds = _timer % 60f;
            //Debug.Log(_seconds);
            if (_timer > DAMAGETIMER)
            {
                //Debug.Log(_seconds);
                LD.isLit = false;
                PositionReset();
            }
        }
    }

    void PositionReset()
    {
        player.position = _reset_position;
    }
}
