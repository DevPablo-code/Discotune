using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = System.Random;

public class HumanRequire
{
    public float RequiredEnergy;
    public float RequiredVolume;
}

public class SpawnerManager : MonoBehaviour
{
    public int MaxHumans = 25;

    public int WavesCount = 3;

    public float WaveDelay = 15f;

    public float PercentOfNonConformists = 0.25f;

    public AnimationCurve SpawnDistribution;

    public GameObject PrefabHumanAI;

    public String humanSpritesheetPath;

    private GameObject spawnParent;

    private List<HumanRequire> RequiresList = new List<HumanRequire>();

    private Sprite[] humansSprites;

    private bool lastWaveSpawned = false;

    void Start()
    {
        spawnParent = new GameObject("HUMANS");

        humansSprites = Resources.LoadAll<Sprite>(humanSpritesheetPath);

        manager_controller djC = FindObjectOfType<manager_controller>();

        float defaultEnergy = UnityEngine.Random.RandomRange(10, 100);
        float defaultVolume = UnityEngine.Random.RandomRange(10, 100);

        for (int i = 1; i <= MaxHumans; ++i)
        {
            HumanRequire r = new HumanRequire();

            if (((float)i / MaxHumans) <= (1f - PercentOfNonConformists))
            {
                r.RequiredEnergy = defaultEnergy;
                r.RequiredVolume = defaultVolume;
            }
            else
            {
                r.RequiredEnergy = UnityEngine.Random.RandomRange(10, 100);
                r.RequiredVolume = UnityEngine.Random.RandomRange(10, 100);
            }

            RequiresList.Add(r);
        }
        
        Random rng = new Random();
        RequiresList = RequiresList.OrderBy(x => rng.Next()).ToList();
        
        spawnWaves();
    }

    private void spawnWaves()
    {
        StartCoroutine(spawnWavesImpl());
    }

    IEnumerator spawnWavesImpl()
    {
        int objectsPerWave = Mathf.RoundToInt((float)MaxHumans / WavesCount);
        int objectsSpawned = 0;

        for (int i = 0; i < WavesCount; ++i)
        {
            float spawnRate = SpawnDistribution.Evaluate((float)objectsSpawned / MaxHumans);

            int objectsToSpawnInWave = Mathf.RoundToInt(spawnRate * objectsPerWave);

            int os = 0;
            for (; os < objectsToSpawnInWave; ++os)
            {
                if (os + objectsSpawned > MaxHumans)
                {
                    --os;
                    break;
                }

                spawnHuman(RequiresList[objectsSpawned]);

                objectsSpawned++;
            }

            //Debug.Log("Human spawned in " + i.ToString() + " waves: " + os.ToString());

            yield return new WaitForSeconds(WaveDelay);
        }

        for(int i = 0; i < MaxHumans - objectsSpawned; ++i)
        {
            spawnHuman(RequiresList[objectsSpawned]);

            objectsSpawned++;
        }

        lastWaveSpawned = true;

        //Debug.Log("Last wave: " + (MaxHumans - objectsSpawned).ToString());

    }

    private void spawnHuman(HumanRequire r)
    {
        Vector3 spawnPos = Vector3.zero;

        spawnPos.x = UnityEngine.Random.RandomRange(0, Screen.width);
        spawnPos.y = Screen.height + 20f;

        spawnPos = Camera.main.ScreenToWorldPoint(spawnPos);
        spawnPos.z = 0f;

        GameObject go = Instantiate(PrefabHumanAI, spawnParent.transform);
        go.transform.position = spawnPos;

        DanceFloorHumanAI human = go.GetComponent<DanceFloorHumanAI>();
        human.RequiredEnergy = r.RequiredEnergy;
        human.RequiredVolume = r.RequiredVolume;
        human.GetComponent<SpriteRenderer>().sprite = humansSprites[UnityEngine.Random.RandomRange(0, humansSprites.Length - 1)];
    }

    public bool IsLastWaveSpawned()
    {
        return lastWaveSpawned;
    }
}
