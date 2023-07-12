using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class knob_Script : MonoBehaviour
{
    [SerializeField] Transform handle;
    [SerializeField] Text ValText;

    Vector3 mousePos;

    public float KnobValue;

    public void OnHandleDrag()
    {
        mousePos = Input.mousePosition;
        Vector2 dir = mousePos - handle.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle <= 0) ? (360 + angle) : angle;

        if(angle <= 225 || angle >= 315)
        {
            Quaternion r = Quaternion.AngleAxis(angle + 135f, Vector3.forward);
            handle.rotation = r;

            angle = ((angle >= 315) ? (angle - 360) : angle) + 45f;
            float fill__amount = 0.75f - (angle / 360f);

            ValText.text = Mathf.Round((fill__amount * 100) / 0.75f).ToString();

            KnobValue = Mathf.Round((fill__amount * 100) / 0.75f) / 100.0f;
        }
    }
}
