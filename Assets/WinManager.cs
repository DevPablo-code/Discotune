using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WinManager : MonoBehaviour
{
    public UnityEngine.UI.Slider ProgressBar;
    public float FineMultiplier = 2f;
    public float AlivePrecentToWin = 0.5f;

    public Canvas WinScreen;
    public Canvas LoseScreen;

    private float decrementAmount = 0f;
    private float winPercent = 1f;
    private SpawnerManager spawnManager;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnerManager>();

        AlivePrecentToWin = Mathf.Max(AlivePrecentToWin, spawnManager.PercentOfNonConformists * 1.5f);
        decrementAmount = 1f / ((float)spawnManager.MaxHumans * AlivePrecentToWin);
    }

    void Update()
    {
        ProgressBar.value = winPercent;

        if(winPercent <= 0f)
        {
            LaunchLoseBehaviour();
        }

        if(spawnManager.IsLastWaveSpawned())
        {
            bool everyoneIsGood = true;

            foreach(var e in FindObjectsOfType<DanceFloorHumanAI>())
            {
                if(e.GetRequestState() != RequestState.NONE)
                {
                    everyoneIsGood = false;
                    break;
                }
            }

            if(everyoneIsGood)
            {
                LaunchWinBehaviour();
            }
        }
    }

    public void Killed(bool fine = false)
    {
        if(fine)
        {
            winPercent -= decrementAmount * FineMultiplier;
        }
        else
        {
            winPercent -= decrementAmount;
        }
    }

    private void LaunchWinBehaviour()
    {
        WinScreen.enabled = true;
    }

    private void LaunchLoseBehaviour()
    {
        LoseScreen.enabled = true;
        Debug.Log("You lost");
    }
}
