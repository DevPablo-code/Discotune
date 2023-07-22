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
    NONE,
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
    public float GlobalSpeed = 5.0f;
    public float GlobalDelayBetweenRoam = 1f;
    public float GlobalRoamDistanceDivide = 5f;

    public float TolerateAIRequire = 15f;
    public float FirstStageWarningTime = 15f;
    public float SecondStageWarningTime = 5f;

    public RequestIcon[] IconsList;

    public Dictionary<RequestType, Sprite> iconsLookupByType = new Dictionary<RequestType, Sprite>();

    private void Start()
    {
        foreach(var e in IconsList)
        {
            iconsLookupByType.Add(e.Type, e.Icon);
        }
    }

    public Vector3 GetRoamPoint(Vector3 Origin)
    {
        float width = Screen.width;
        float height = Screen.height * 0.35f;

        float randW = Random.RandomRange(20, width - 20);
        float randH = Random.RandomRange(Screen.height - 30, height);

        Vector3 screenPoint = new Vector3(randW, randH, 0);

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        worldPoint.z = 0;

        return worldPoint;
    }
}
