using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ControlledLightColorMode
{
    Fixed,
    Dynamic,
}

[System.Serializable]
public class ControlledLight
{
    public Light2D lightSource;
    public ControlledLightColorMode colorMode;

    public float cycleInterval;
    

    public Color startColor;
    public bool randomStartColor;
}
