using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class KnobEvent : UnityEvent<float>
{

}

public class KnobHandle : MonoBehaviour
{
    public KnobEvent OnKnobDrag;

    public Text ValueText;

    private Transform handle;
    private Vector3 mousePos;

    private float knobValue = 0.0f;

    public void Start()
    {
        handle = this.transform;
    }

    public void OnHandleDrag()
    {
        mousePos = Input.mousePosition;
        Vector2 dir = mousePos - handle.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle <= 0) ? (360 + angle) : angle;

        if (angle <= 225 || angle >= 315)
        {
            Quaternion r = Quaternion.AngleAxis(angle + 135f, Vector3.forward);
            handle.rotation = r;

            angle = ((angle >= 315) ? (angle - 360) : angle) + 45f;
            float fill__amount = 0.75f - (angle / 360f);

            knobValue = fill__amount / 0.75f;
            OnValueChanged(knobValue);
        }
    }

    public virtual void OnValueChanged(float NewValue)
    {
        ValueText.text = Mathf.Round(NewValue * 100.0f).ToString();
        OnKnobDrag.Invoke(NewValue);
    }

    public float getLastKnobValue()
    {
        return knobValue;
    }
}
