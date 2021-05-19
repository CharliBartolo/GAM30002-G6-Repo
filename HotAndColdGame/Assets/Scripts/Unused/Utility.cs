using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static bool Approximately(float a, float b, float t)
    {
        return Mathf.Abs(a - b) <= t;
    }
}