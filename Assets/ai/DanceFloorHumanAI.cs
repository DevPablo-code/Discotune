using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloorHumanAI : MonoBehaviour
{

    public GameObject bloodParticle;

    public float health
    {
        get
        {
            return _health;
        }
    }


    private Vector3 roamDestination;
    private ManagerAI aiManager;

    private bool needNewPoint = false;
    [SerializeField]
    private float _health = 100f;

    void Start()
    {
        aiManager = FindObjectOfType<ManagerAI>();

        roamDestination = aiManager.GetRoamPoint(this.transform.position);

        Animation();
    }

    void Update()
    {
        if(this.transform.position != roamDestination)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, roamDestination, aiManager.GlobalSpeed * Time.deltaTime);
            needNewPoint = true;
        }
        else
        {
            if(needNewPoint)
            {
                Invoke("UpdateRoamPoint", aiManager.GlobalDelayBetweenRoam);
                needNewPoint = false;
            }
        }
    }

    private void UpdateRoamPoint()
    {
        roamDestination = aiManager.GetRoamPoint(this.transform.position);
    }

    private void Animation()
    {
        this.GetComponent<SpriteRenderer>().flipX = !this.GetComponent<SpriteRenderer>().flipX;
        Invoke("Animation", 0.8f);
    }

    private void Die() 
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 blooParticlePosition = new Vector2(bounds.center.x + Random.Range(-0.4f, 0.4f), bounds.min.y + Random.Range(-0.4f, 0.4f));
        gameObject.SetActive(false);

        GameObject spawnedBlood = Instantiate(bloodParticle, blooParticlePosition, Quaternion.identity);
        spawnedBlood.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.4f, 1f), 0f, 0f, 1.0f);
        spawnedBlood.transform.Rotate(Random.Range(0f, 30f), 0, Random.Range(-5f, 5f), Space.Self);

        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount) 
    {
        _health -= damageAmount;
        if(_health <= 0) 
        {
            Die();
        }
    }
}
