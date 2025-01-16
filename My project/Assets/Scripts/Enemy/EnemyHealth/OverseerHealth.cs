using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverseerHealth : Health
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
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
            //attack1?.Invoke();
            attack2?.Invoke();
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= overseer.attackRange)
        {
            player.TakeDamage(Unit.Damage);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > overseer.attackRange)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
            else
                Debug.Log("passive already active");
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
        if (canDie)
        {
            deathLogic.KilledWhenDeathDefiance();
            spawner.mindOnField--;
        }
    }
}
