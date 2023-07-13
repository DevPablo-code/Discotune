using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerAI : MonoBehaviour
{
    public BoxCollider2D AreaCollide;

    public float GlobalSpeed = 5.0f;
    public float GlobalDelayBetweenRoam = 1f;
    public float GlobalRoamDistanceDivide = 5f;

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
