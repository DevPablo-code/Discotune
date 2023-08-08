using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public enum TrackEnergy
{
    LOW,
    HIGH,
    EXTREME
}

[System.Serializable]
public class TrackInfo
{
    public AudioClip TrackSource;
    public TrackEnergy Energy;
}

public class manager_controller : MonoBehaviour
{
    public AudioSource MainAS;

    public TrackInfo[] TrackList;

    public KnobHandle BPmHandle;
    public float MinBPM = 0.5f;
    public float MaxBPM = 1.5f;

    public KnobHandle VolumeHandle;
    public float MinVolume = 0.1f;
    public float MaxVolume = 1.0f;

    private float currentVolume = 0.0f;
    private float currentEnergy = 0.0f;

    void Start()
    {
        BPmHandle.OnKnobDrag.AddListener(OnBPMChanged);
        VolumeHandle.OnKnobDrag.AddListener(OnVolumeChanged);

        BPmHandle.OnValueChanged(0.0f);
        VolumeHandle.OnValueChanged(0.0f);
    }

    public float GetCurrentVolume()
    {
        return currentVolume;
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    private void OnBPMChanged(float Val)
    {
        float bpm = MinBPM + ((MaxBPM - MinBPM) * Val);
        MainAS.pitch = bpm;
        MainAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / bpm);

        float trackVal = Mathf.Round(((Val * 100.0f) / (100.0f / (TrackList.Length - 1))));
        var track = TrackList[(int)trackVal].TrackSource;
        if(MainAS.clip != track)
        {
            MainAS.clip = TrackList[(int)trackVal].TrackSource;
            MainAS.Play();
        }

        computeEnergy(bpm);
    }

    private void OnVolumeChanged(float Val)
    {
        float volumeNorm = MinVolume + ((MaxVolume - MinVolume) * Val);
        MainAS.volume = volumeNorm;

        currentVolume = Mathf.Round(volumeNorm * 100f);
    }

    private void computeEnergy(float bpm)
    {
        //ENERGY COMPUTATION HAPPENS HERE
        //
        //
        float energyTypeMult = 1.0f;
        currentEnergy = Mathf.Round(bpm * 100f * energyTypeMult);
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(10, 0, 150, 150), "Energy: " + currentEnergy.ToString());
        //GUI.Label(new Rect(10, 20, 150, 150), "Volume: " + currentVolume.ToString());
    }
}