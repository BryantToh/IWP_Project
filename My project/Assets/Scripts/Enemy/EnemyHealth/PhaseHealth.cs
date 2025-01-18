using System.Collections.Generic;
using UnityEngine;

public class PhaseHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    PhaseEnemy phase;
    DeathLogic deathLogic;
    SurgeLogic surgeLogic;
    protected override void Start()
    {
        base.Start();
    }

    public void OnEnemySpawn()
    {
        currentHealth = Unit.Health;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        phase = GetComponent<PhaseEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }

    public void AttackPlayer(Collider other)
    {
        if (!phase.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= phase.attackRange)
        {
            player.TakeDamage(Unit.Damage);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > phase.attackRange)
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
            spawner.phaseOnField--;
            ObjectPooler.Instance.Release("phase", this);
        }
    }

    public void OnGet()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public void OnDestroyInterface()
    {
        Destroy(gameObject);
    }

    public void SetPosNRot(Vector3 Pos, Quaternion Rot)
    {
        transform.position = Pos;
        transform.rotation = Rot;
    }
}
