using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AngryDanceFloorHumanAI : DanceFloorHumanAI
{
    public float attackCooldown = 2.0f;
    public float attackDamage = 50f;
    public AudioClip attackSound;
    public AudioSource _audioSource;

    private ManagerAI aiManager;
    private DanceFloorHumanAI attackTarget;
    private float attackTime = 0.0f;
    private bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        aiManager = FindObjectOfType<ManagerAI>();

        _audioSource = GetComponent<AudioSource>();

        ChooseAttackTarget();

        canAttack = true;

        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackTarget)
        {
            ChooseAttackTarget();
        }
        else
        {
            if (Vector2.Distance(this.transform.position, attackTarget.transform.position) <= 1)
            {
                if (canAttack) 
                {
                    Attack();

                    canAttack = false;
                }
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, attackTarget.transform.position, aiManager.GlobalSpeed * 1.5f * Time.deltaTime);
            }
        }
        if (!canAttack && ((Time.fixedTime - attackTime) >= attackCooldown))
        {
            canAttack = true;
        }
    }

    private void ChooseAttackTarget() 
    {
        var peopleOnDanceFloor = GameObject.FindObjectsOfType<DanceFloorHumanAI>();

        attackTarget = peopleOnDanceFloor.Where(obj => obj != this).OrderBy(obj => (obj.transform.position - this.transform.position).sqrMagnitude).FirstOrDefault();
    }

    private void Attack() 
    {
        if (attackTarget) 
        {
            attackTime = Time.fixedTime;
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(attackSound);
            }
            attackTarget.TakeDamage(attackDamage);
        }
    }
}
