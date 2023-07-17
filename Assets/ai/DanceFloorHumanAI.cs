using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DanceFloorHumanAI : MonoBehaviour
{
    public SpriteRenderer BubbleChatSprite;
    public SpriteRenderer ChatInfoSprite;


    private Vector3 roamDestination;
    private ManagerAI aiManager;

    private bool needNewPoint = false;

    private RequestState requestState;

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

        if(requestState != RequestState.NONE)
        {
            BubbleChatSprite.enabled = true;
            ChatInfoSprite.enabled = true;
        }
        else
        {
            BubbleChatSprite.enabled = false;
            ChatInfoSprite.enabled = false;
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

    private void MakeRequestDelayed()
    {
        Invoke("ImmediateRequest", Random.RandomRange(1.0f, 15.0f));
    }

    private void ImmediateRequest()
    {
        requestState = RequestState.FIRST_WARNING;
    }
}
