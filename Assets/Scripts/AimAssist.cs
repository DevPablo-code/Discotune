using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public Gun gun;
    public float assistDistance = 0.5f;

    private bool crosshairDelay;

    // Start is called before the first frame update
    void Start()
    {
        crosshairDelay = gun.crosshairDelay;
        gameObject.SetActive(PlayerPrefs.GetInt("AimAssist") > 0);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(Camera.main.ScreenToWorldPoint(gun.crosshair.transform.position), assistDistance, Vector2.zero);
        if (hits.Any(hit => hit.transform.gameObject.GetComponent<DanceFloorHumanAI>() != null)) 
        {
            gun.crosshairDelay = true; //gun.crosshair.transform.position = hits[0].transform.position;
        }
        else 
        {
            gun.crosshairDelay = crosshairDelay;
        }
    }
}
