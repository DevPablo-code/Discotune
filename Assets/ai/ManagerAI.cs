using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RequestState
{
    NONE,
    FIRST_WARNING,
    LAST_WARNING,
    KILLING
}

public enum RequestType
{
    MUSIC_TOO_LOUD = 0,
    MUSIC_TOO_QUIET,
    MUSIC_TOO_SLOW,
    MUSIC_TOO_FAST
}

[System.Serializable]
public class RequestIcon
{
    public RequestType Type;
    public Sprite Icon;
}

public class ManagerAI : MonoBehaviour
{
    public BoxCollider2D AreaCollide;

    public float GlobalSpeed = 5.0f;
    public float GlobalDelayBetweenRoam = 1f;
    public float GlobalRoamDistanceDivide = 5f;

    public RequestIcon[] IconsList;

    private Dictionary<RequestType, Sprite> iconsLookupByType = new Dictionary<RequestType, Sprite>();

    private void Start()
    {
        foreach(var e in IconsList)
        {
            iconsLookupByType.Add(e.Type, e.Icon);
        }
    }

    public Vector3 GetRoamPoint(Vector3 Origin)
    {
        Bounds bounds = AreaCollide.bounds;

        Vector3 boundsVector = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
           0);

        Vector3 diff = boundsVector - Origin;

        return Vector3.Normalize(diff) * (diff.magnitude / GlobalRoamDistanceDivide);
    }
}
