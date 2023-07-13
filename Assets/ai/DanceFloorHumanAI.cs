using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloorHumanAI : MonoBehaviour
{
    private Vector3 roamDestination;
    private ManagerAI aiManager;

    private bool needNewPoint = false;

    void Start()
    {
        aiManager = FindObjectOfType<ManagerAI>();

        roamDestination = aiManager.GetRoamPoint(this.transform.position);

        Animation();
    }

    void Update()
    {
        if(this.transform.position != roamDestination)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, roamDestination, aiManager.GlobalSpeed * Time.deltaTime);
            needNewPoint = true;
        }
        else
        {
            if(needNewPoint)
            {
                Invoke("UpdateRoamPoint", aiManager.GlobalDelayBetweenRoam);
                needNewPoint = false;
            }
        }
    }

    private void UpdateRoamPoint()
    {
        roamDestination = aiManager.GetRoamPoint(this.transform.position);
    }

    private void Animation()
    {
        this.GetComponent<SpriteRenderer>().flipX = !this.GetComponent<SpriteRenderer>().flipX;
        Invoke("Animation", 0.8f);
    }
}
