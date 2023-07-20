using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EnergyKnobHandler : KnobHandle
{
    public TrackEnergy CurrentEnergy = TrackEnergy.LOW;

    private manager_controller managerController;

    public void Awake()
    {
        managerController = FindObjectOfType<manager_controller>();
    }

    public override void OnValueChanged(float NewValue)
    {
        float val = Mathf.Round(((NewValue * 100.0f) / (100.0f / (managerController.TrackList.Length - 1))));
        OnKnobDrag.Invoke(val);

        CurrentEnergy = managerController.TrackList[(int)val].Energy;
        ValueText.text = CurrentEnergy.ToString();
    }
}
