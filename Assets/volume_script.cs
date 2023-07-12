using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volume_script : MonoBehaviour
{
    public float MinVolume = 0.1f;
    public float MaxVolume = 1f;

    public knob_Script KnobS;
    public AudioSource MainAS;

    void Start()
    {
        KnobS = this.GetComponent<knob_Script>();
    }

    void Update()
    {
        MainAS.volume = MinVolume + ((MaxVolume - MinVolume) * KnobS.KnobValue);
    }
}
