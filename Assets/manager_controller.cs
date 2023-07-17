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

    public KnobHandle EnergyHandle;

    public KnobHandle BPmHandle;
    public float MinBPM = 0.5f;
    public float MaxBPM = 1.5f;

    public KnobHandle VolumeHandle;
    public float MinVolume = 0.1f;
    public float MaxVolume = 1.0f;

    void Start()
    {
        EnergyHandle.OnKnobDrag.AddListener(OnEnergyChanged);
        BPmHandle.OnKnobDrag.AddListener(OnBPMChanged);
        VolumeHandle.OnKnobDrag.AddListener(OnVolumeChanged);

        EnergyHandle.OnValueChanged(0.0f);
        BPmHandle.OnValueChanged(0.0f);
        VolumeHandle.OnValueChanged(0.0f);
    }

    private void OnEnergyChanged(float Val)
    {
        MainAS.clip = TrackList[(int)Val].TrackSource; ;
        MainAS.Play();
    }

    private void OnBPMChanged(float Val)
    {
        float bpm = MinBPM + ((MaxBPM - MinBPM) * Val);
        MainAS.pitch = bpm;
        MainAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / bpm);

    }

    private void OnVolumeChanged(float Val)
    {
        MainAS.volume = MinVolume + ((MaxVolume - MinVolume) * Val);
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 0, 150, 150), "Energy: ");
        GUI.Label(new Rect(10, 20, 150, 150), "Volume: ");
    }

}