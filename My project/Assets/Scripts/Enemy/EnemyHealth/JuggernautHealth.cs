using System.Collections.Generic;
using UnityEngine;

public class JuggernautHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    JuggernautEnemy juggernaut;
    DeathLogic deathLogic;
    SurgeLogic surgeLogic;
    protected override void Start()
    {
        base.Start();
    }

    public void OnEnemySpawn()
    {
        pooler = ObjectPooler.Instance;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        juggernaut = GetComponent<JuggernautEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }

    public void AttackPlayer(Collider other)
    {
        if (!juggernaut.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= juggernaut.attackRange)
        {
            player.TakeDamage(Unit.Damage);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > juggernaut.attackRange)
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
        if (canDie && !isReleased) // Ensure release is only called once
        {
            ObjectPooler.Instance.Release("juggernaut", this);
            isReleased = true; // Set to true to prevent further releases
            deathLogic.KilledWhenDeathDefiance();
            spawner.juggernautOnField--;
        }
    }

    public void OnGet()
    {
        currentHealth = Unit.Health;
        //pooler.isPooled = true;
        isReleased = false;
        canDie = false;
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        pooler.isPooled = false;
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
