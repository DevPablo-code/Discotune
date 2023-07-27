using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject flash;
    public AudioClip shotSound;
    public float HideTimeout = 5f;
    public float FlashTime = 0.2f;
    public bool autoEquip = false;
    public float damage = 50f;
    public bool crosshairDelay = false;

    private bool _equipped;
    private AudioSource _audioSource;
    private Vector2 _shootAtPosition;
    private float shootTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        gameObject.GetComponent<Image>().enabled = false;

        if(autoEquip)
        {
            Equip();
        }
        else
        {
            Unequip();
        }
    }

    // Called every frame
    void Update() 
    {
        if (_equipped) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            else 
            {
                if (crosshairDelay) 
                {
                    crosshair.transform.position = Vector2.Lerp(crosshair.transform.position, Input.mousePosition, 5f * Time.deltaTime);
                }
                else 
                {
                    crosshair.transform.position = Input.mousePosition;
                }
               
                _shootAtPosition = Camera.main.ScreenToWorldPoint(crosshair.transform.position);
            }
        }

        if((Time.fixedTime - shootTime) >= FlashTime) 
        {
            flash.SetActive(false);
        }
        if((Time.fixedTime - shootTime) >= HideTimeout)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
    }

    public void Equip() 
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
        _equipped = true;
    }

    public void Unequip()
    {
        crosshair.SetActive(false);
        Cursor.visible = true;
        _equipped = false;
    }

    public void Shoot() 
    {
        if (_equipped) 
        {
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(shotSound);
            }

            shootTime = Time.fixedTime;

            gameObject.GetComponent<Image>().enabled = true;


            flash.SetActive(true);

            RaycastHit2D hit = Physics2D.Raycast(_shootAtPosition, Vector2.zero);

            if (hit.collider != null) 
            {
                Debug.Log(hit.transform.gameObject.name);
                if(hit.transform.gameObject.TryGetComponent(out DanceFloorHumanAI target)) 
                {
                    target.TakeDamage(damage, true);
                }  
            }
        }
    }
}
