using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject flash;
    public AudioClip ShotSound;

    private bool _equipped;
    private AudioSource _audioSource;
    private Vector2 _shootAtPosition;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Equip();
        flash.SetActive(false);
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
                crosshair.transform.position = Vector2.Lerp(crosshair.transform.position, Input.mousePosition, 5f * Time.deltaTime);
                _shootAtPosition = Camera.main.ScreenToWorldPoint(crosshair.transform.position);
            }
        }
    }

    public void Equip() 
    {
        this.gameObject.SetActive(true);
        crosshair.SetActive(true);
        _equipped = true;
    }

    public void Unequip()
    {
        this.gameObject.SetActive(false);
        crosshair.SetActive(false);
        _equipped = false;
    }

    public GameObject Shoot() 
    {
        if (_equipped) 
        {
            if (_audioSource != null) 
            {
                _audioSource.PlayOneShot(ShotSound);

                Flash();

                RaycastHit2D hit = Physics2D.Raycast(_shootAtPosition, Vector2.zero);

                return hit.transform.gameObject;
            }
        }
        return null;
    }

    private void Flash() 
    {
        flash.SetActive(true);
        Invoke("HideFlash", 0.2f);
    }

    private void HideFlash() 
    {
        flash.SetActive(false);
    }
}
