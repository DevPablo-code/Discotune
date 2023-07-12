using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bpm_script : MonoBehaviour
{
    public float MinMultiplier = 0.5f;
    public float MaxMultiplier = 2.0f;

    public knob_Script KnobS;
    public AudioSource MainAS;

    void Start()
    {
        KnobS = this.GetComponent<knob_Script>();
    }

    void Update()
    {
        float newSpeed = MinMultiplier + ((MaxMultiplier - MinMultiplier) * KnobS.KnobValue);
        MainAS.pitch = newSpeed;
        MainAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / newSpeed);
    }
}
