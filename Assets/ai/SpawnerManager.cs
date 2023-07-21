using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public int MaxHumans = 25;

    public int WavesCount = 3;

    public float WaveDelay = 15f;

    public GameObject PrefabHumanAI;

    private Camera MainCam;

    void Start()
    {
        MainCam = FindObjectOfType<Camera>();

        spawnWaves();
    }

    private void spawnWaves()
    {
        StartCoroutine(spawnWavesImpl());
    }

    IEnumerator spawnWavesImpl()
    {
        for (int i = 0; i < WavesCount; ++i)
        {
            int objectsCount = (int)Mathf.Round(MaxHumans / WavesCount);
            for(int o = 0; o < objectsCount; ++o)
            {
                spawnHuman();
            }

            yield return new WaitForSeconds(WaveDelay);
        }
    }

    private void spawnHuman()
    {
        Vector3 spawnPos = Vector3.zero;

        spawnPos.x = Random.RandomRange(0, Screen.width);
        spawnPos.y = Screen.height + 20f;

        spawnPos = MainCam.ScreenToWorldPoint(spawnPos);
        spawnPos.z = 0f;

        Instantiate(PrefabHumanAI, spawnPos, Quaternion.identity); ;
    }
}
