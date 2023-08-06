using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// DEFINITELLY AN OVERKILL BUT I DONT WANT TO LEAVE IT ALONE

public class LightsController : MonoBehaviour
{
    public ControlledLight[] controlledLights;

    private Color[] colors = { new Color(0, 0, 1, 1), new Color(1, 0, 0, 1), new Color(1, 0, 1, 1), new Color(0.54f, 0, 1, 1), new Color(1, 0, 0.73f, 1), new Color(0, 0.7f, 1, 1) };

    void Start()
    {
        foreach (var light in controlledLights) 
        {
            if(light.randomStartColor) 
            {
                light.lightSource.color = colors[Random.Range(0, colors.Length)];
            }
            else 
            {
                light.lightSource.color = light.startColor;
            }
            StartCoroutine(UpdateColor(light));
        }
    }
    IEnumerator UpdateColor(ControlledLight light)
    {
        switch (light.colorMode)
        {
            case ControlledLightColorMode.Fixed:
            {
                yield return null;
                break;
            }
            case ControlledLightColorMode.Dynamic:
            {
                yield return new WaitForSeconds(light.cycleInterval);
                light.lightSource.color = colors[Random.Range(0, colors.Length)];
                StartCoroutine(UpdateColor(light));
                Debug.Log(light.lightSource.color);
                break;
            }
        }
    }
}
