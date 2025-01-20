using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent attack1;
    public UnityEvent attack2;
    public UnityEvent attack3;
    public UnityEvent attack4;
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        overseer = GetComponent<OverseerEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
        laserBeam.SetActive(false);
    }
    //public void OnEnemySpawn()
    //{

    //}

    //public void OnGet()
    //{
    //    gameObject.SetActive(true);
    //}

    //public void OnRelease()
    //{
    //    gameObject.SetActive(false);
    //}

    //public void OnDestroyInterface()
    //{
    //    Destroy(gameObject);
    //}

    //public void SetPosNRot(Vector3 Pos, Quaternion Rot)
    //{
    //    transform.position = Pos;
    //    transform.rotation = Rot;
    //}
    public void AttackPlayer(Collider other)
    {
        if (!overseer.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            attack2?.Invoke();

            //int randomAttack = Random.Range(0, 3);

            //switch (randomAttack)
            //{
            //    case 0:
            //        overseer.attackRange = overseer.rangedAttack;
            //        attack1?.Invoke();
            //        break;
            //    case 1:
            //        overseer.attackRange = overseer.rangedAttack;
            //        break;
            //    case 2:
            //        overseer.attackRange = overseer.meleeAttack;
            //        attack4?.Invoke();
            //        break;
            //}
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
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth == Unit.Health * 0.25f || currentHealth == Unit.Health * 0.5f || currentHealth == Unit.Health * 0.75f)
        {
            attack3?.Invoke();
        }
        if (canDie)
        {
            deathLogic.KilledWhenDeathDefiance();
            spawner.mindOnField--;
        }
    }
}
