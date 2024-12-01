using System.Collections.Generic;
using UnityEngine;

public class JuggernautHealth : Health
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    JuggernautEnemy juggernaut;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        juggernaut = GetComponent<JuggernautEnemy>();
        base.Start();
    }

    public void AttackPlayer(Collider other)
    {
        if (!juggernaut.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            player.TakeDamage(Unit.Damage);
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
            spawner.juggernautOnField--;
            gameObject.SetActive(false);
        }
    }
}
