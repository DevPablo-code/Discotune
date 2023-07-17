using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject flash;
    public AudioClip shotSound;
    public GameObject bloodParticle;
    public float HideTimeout = 5f;
    public float FlashTime = 0.2f;
    public bool autoEquip = false;

    private bool _equipped;
    public AudioSource _audioSource;
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
                crosshair.transform.position = Vector2.Lerp(crosshair.transform.position, Input.mousePosition, 5f * Time.deltaTime);
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
        _equipped = true;
    }

    public void Unequip()
    {
        crosshair.SetActive(false);
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
                Vector2 blooParticlePosition = new Vector2(hit.collider.bounds.center.x + Random.Range(-0.4f, 0.4f), hit.collider.bounds.min.y + Random.Range(-0.4f, 0.4f));
                hit.transform.gameObject.SetActive(false);

                GameObject spawnedBlood = Instantiate(bloodParticle, blooParticlePosition, Quaternion.identity);
                spawnedBlood.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.4f, 1f), 0f, 0f, 1.0f);
                spawnedBlood.transform.Rotate(Random.Range(0f, 30f), 0, Random.Range(-5f, 5f), Space.Self);
            }
        }
    }
}
