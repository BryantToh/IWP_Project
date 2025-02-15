using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OverseerHealth : Health
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    public Dashing playerDash;
    OverseerEnemy overseer;
    DeathLogic deathLogic;
    SurgeLogic surgeLogic;
    public LayerMask playerLayer;
    public GameObject laserBeam;
    public Transform rangeAttackSpawn;
    public float maxBeamDist;
    public bool laserShot = false;
    public Animator animator;
    public BossAttack_Punch attackPunch;
    public bool isDead = false;
    bool pushUsed = false;
    public Slider healthSlider;
    public UnityEvent attack1;
    public UnityEvent attack2;
    public UnityEvent attack3;
    public UnityEvent attack4;
    protected override void Start()
    {
        base.Start();
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        overseer = GetComponent<OverseerEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
        laserBeam.SetActive(false);
    }
    public void AttackPlayer(Collider other)
    {
        if (!overseer.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);

            int randomAttack = Random.Range(0, 3);
            switch (randomAttack)
            {
                case 0:
                    overseer.attackRange = overseer.rangedAttack;
                    animator.SetTrigger("Laser");
                    attack1?.Invoke();
                    break;
                case 1:
                    overseer.attackRange = overseer.rangedAttack;
                    animator.SetTrigger("Missile");
                    attack2?.Invoke();
                    break;
                case 2:
                    overseer.attackRange = overseer.meleeAttack;
                    animator.SetTrigger("Punch");
                    attack4?.Invoke();
                    break;
            }
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= overseer.attackRange && !playerDash.isDashing)
        {
            player.TakeDamage(Unit.Damage);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= overseer.attackRange && playerDash.isDashing ||
            Vector3.Distance(player.transform.position, transform.position) > overseer.attackRange && playerDash.isDashing)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
            else
                Debug.Log("passive already active");
        }
        else
        {
            Debug.Log("player out of range");
        }
    }
    public void AttackReset(Collider other)
    {
        if (damageSources.Contains(other))
        {
            damageSources.Remove(other);
            attackPunch.NotPunching();
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthSlider.value -= damage;
        if (currentHealth <= Unit.Health * 0.5f && !pushUsed)
        {
            attack3?.Invoke();
            pushUsed = true;
        }

        if (currentHealth <= 0f)
        {
            isDead = true;
        }
    }
}
