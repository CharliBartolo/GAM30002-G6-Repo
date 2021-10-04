using System;
using UnityEngine;
using UnityStandardAssets.Utility;

    public class HeadBob : MonoBehaviour
    {
        public Camera Camera;
        public CurveControlledBob motionBob = new CurveControlledBob();
        public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
        public Rigidbody playerRigidbody;
        public float StrideInterval;
        [Range(0f, 1f)] public float RunningStrideLengthen;

       // private CameraRefocus m_CameraRefocus;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;


        private void Start()
        {
            motionBob.Setup(Camera, StrideInterval);
            m_OriginalCameraPosition = Camera.transform.localPosition;
       //     m_CameraRefocus = new CameraRefocus(Camera, transform.root.transform, Camera.transform.localPosition);
        }


        public void UpdateHeadBob(Vector3 horizVelocity, bool Grounded)
        {
        //  m_CameraRefocus.GetFocusPoint();
        Debug.Log("Headbob");
            Vector3 newCameraPosition;
            if (horizVelocity.magnitude > 0.1f && Grounded)
            {
                Camera.transform.localPosition = motionBob.DoHeadBob(horizVelocity.magnitude);
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
            }
            else
            {
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
            }
            Camera.transform.localPosition = newCameraPosition;

            if (!m_PreviouslyGrounded && Grounded)
            {
                StartCoroutine(jumpAndLandingBob.DoBobCycle());
            }

            m_PreviouslyGrounded = Grounded;
          //  m_CameraRefocus.SetFocusPoint();
        }
    }
