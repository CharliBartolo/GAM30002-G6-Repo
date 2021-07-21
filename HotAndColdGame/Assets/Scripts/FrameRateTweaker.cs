#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateTweaker : MonoBehaviour
{
    public int vSyncValue, fpsLimit;

    // Start is called before the first frame update
    void Start()
    {
        vSyncValue = QualitySettings.vSyncCount;
        fpsLimit = Application.targetFrameRate;
    }

    // Update is called once per frame
    void Update()
    {
        QualitySettings.vSyncCount = vSyncValue;
        Application.targetFrameRate = fpsLimit;
    }
}
#endif