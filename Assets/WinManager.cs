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
    private bool gameEnded = false;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnerManager>();

        AlivePrecentToWin = Mathf.Max(AlivePrecentToWin, spawnManager.PercentOfNonConformists * 1.5f);
        decrementAmount = 1f / ((float)spawnManager.MaxHumans * AlivePrecentToWin);
    }

    void Update()
    {
        if(gameEnded)
        {
            return;
        }

        ProgressBar.value = winPercent;

        if(winPercent <= 0f)
        {
            LaunchLoseBehaviour();
            gameEnded = true;
            return;
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

            foreach(var e in FindObjectsOfType<AngryDanceFloorHumanAI>())
            {
                if(e.enabled)
                {
                    everyoneIsGood = false;
                    break;
                }
            }

            if(everyoneIsGood)
            {
                LaunchWinBehaviour();
                gameEnded = true;
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
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 0f;
    }

    private void LaunchLoseBehaviour()
    {
        LoseScreen.enabled = true;
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
