using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool _enable = true;
    [SerializeField, Range(0f, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0f, 30)] private float _frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private Rigidbody _controller;

    // Start is called before the first frame update
    private void Awake()
    {
        _controller = GetComponent<Rigidbody>();
        _startPos = _camera.localPosition;
    }

    // Update is called once per frame
    public void UpdateHeadbob(bool Grounded)
    {
        if (!Grounded) return;
        if (!_enable) return;

        CheckMotion(Grounded);
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }


    private void CheckMotion(bool Grounded)
    {
        float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;

        if (speed < _toggleSpeed) return;
        if (!Grounded) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }



    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }




    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
}