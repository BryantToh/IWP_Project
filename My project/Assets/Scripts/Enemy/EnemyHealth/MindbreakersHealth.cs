using System.Collections.Generic;
using UnityEngine;

public class MindbreakersHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    MindBreakersEnemy mindBreaker;
    GlitchController glitchCon;
    [Header("Projectile")]
    public GameObject mindProjectile, spawnPoint;
    float projectileSpeed = 3.5f;
    DeathLogic deathLogic;
    public void OnEnemySpawn()
    {
        glitchCon = GameObject.Find("GameController").GetComponent<GlitchController>(); 
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        mindBreaker = GetComponent<MindBreakersEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
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

    public void AttackPlayer(Collider other)
    {
        if (!mindBreaker.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            ShootMindProj();
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
            ObjectPooler.Instance.Release("breaker", this);
        }
    }

    private void ShootMindProj()
    {
        GameObject obj = Instantiate(mindProjectile, spawnPoint.transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (player.gameObject.transform.position - spawnPoint.transform.position).normalized;
            rb.linearVelocity = dir * projectileSpeed;
        }
    }
}
