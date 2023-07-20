using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DanceFloorHumanAI : MonoBehaviour
{
    public SpriteRenderer BubbleChatSprite;
    public SpriteRenderer ChatInfoSprite;

    public float RequiredVolume = 75f;
    public float RequiredEnergy = 75f;

    private Vector3 roamDestination;
    private ManagerAI aiManager;
    private manager_controller djManager;

    private bool needNewPoint = false;

    private RequestState requestState;
    private RequestType requestType;

    private float TimerRequests = 0.0f;

    void Start()
    {
        aiManager = FindObjectOfType<ManagerAI>();
        djManager = FindObjectOfType<manager_controller>();

        roamDestination = aiManager.GetRoamPoint(this.transform.position);

        Animation();
    }

    void Update()
    {
        if(requestState != RequestState.KILLING)
        {
            if (this.transform.position != roamDestination)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, roamDestination, aiManager.GlobalSpeed * Time.deltaTime);
                needNewPoint = true;
            }
            else
            {
                if (needNewPoint)
                {
                    Invoke("UpdateRoamPoint", aiManager.GlobalDelayBetweenRoam);
                    needNewPoint = false;
                }
            }


            if (requestState != RequestState.NONE)
            {
                BubbleChatSprite.enabled = true;
                ChatInfoSprite.enabled = true;
            }
            else
            {
                BubbleChatSprite.enabled = false;
                ChatInfoSprite.enabled = false;
            }

            if (!FastApproximately(RequiredEnergy, djManager.GetCurrentEnergy(), aiManager.TolerateAIRequire))
            {
                ImmediateRequest(djManager.GetCurrentEnergy() < RequiredEnergy ? RequestType.MUSIC_TOO_SLOW : RequestType.MUSIC_TOO_FAST);
            }
            else if (!FastApproximately(RequiredVolume, djManager.GetCurrentVolume(), aiManager.TolerateAIRequire))
            {
                ImmediateRequest(djManager.GetCurrentVolume() < RequiredVolume ? RequestType.MUSIC_TOO_QUIET : RequestType.MUSIC_TOO_LOUD);
            }
            else
            {
                requestState = RequestState.NONE;
                requestType = RequestType.NONE;

                TimerRequests = 0.0f;
            }

            UpdateTimer();
        }
        else
        {
            BubbleChatSprite.enabled = false;
            ChatInfoSprite.enabled = false;

            this.GetComponent<SpriteRenderer>().color = Color.red;

            //KILLING BEHAVIOUR MUST TURN ON HERE
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

    private void ImmediateRequest(RequestType type)
    {
        if(requestType != type)
        {
            requestState = RequestState.FIRST_WARNING;
            requestType = type;

            TimerRequests = 0.0f;

            updateChatInfo(type);
        }
    }

    private void UpdateTimer()
    {
        TimerRequests += Time.deltaTime;

        if(requestState != RequestState.LAST_WARNING)
        {
            if (TimerRequests >= aiManager.FirstStageWarningTime)
            {
                requestState = RequestState.LAST_WARNING;

                BubbleChatSprite.color = Color.red;

                TimerRequests = 0.0f;
            }
        }
        else if(TimerRequests >= aiManager.SecondStageWarningTime)
        {
            requestState = RequestState.KILLING;
            TimerRequests = 0.0f;
        }
    }

    private void updateChatInfo(RequestType type)   
    {
        ChatInfoSprite.sprite = aiManager.iconsLookupByType[type];
        BubbleChatSprite.color = Color.white;
    }
    private bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
