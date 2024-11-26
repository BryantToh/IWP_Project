using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !damageSources.Contains(other))
        {
            damageSources.Add(other);
            player.TakeDamage(Unit.Damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (damageSources.Contains(other))
        {
            damageSources.Remove(other);
        }
    }
}
