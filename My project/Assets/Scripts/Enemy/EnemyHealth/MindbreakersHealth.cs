using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class MindbreakersHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    MindBreakersEnemy mindBreaker;
    public Animator animator;
    [Header("Projectile")]
    public GameObject mindProjectile, spawnPoint;
    float projectileSpeed = 3.5f;
    DeathLogic deathLogic;
    public Slider healthSlider;
    public ParticleSystem particle;
    public void OnEnemySpawn()
    {
        pooler = ObjectPooler.Instance;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        mindBreaker = GetComponent<MindBreakersEnemy>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
    }

    protected override void Start()
    {
        base.Start();
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
    }

    public void OnGet()
    {
        currentHealth = Unit.Health;
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
        pooler.isPooled = true;
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

    public void AttackPlayer(Collider other)
    {
        if (!mindBreaker.playerInAttackRange)
            return;

        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            Instantiate(particle, spawnPoint.transform.position, Quaternion.identity);
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
        healthSlider.value -= damage;
        if (canDie && !isReleased) // Ensure release is only called once
        {
            ObjectPooler.Instance.Release("breaker", this);
            isReleased = true; // Set to true to prevent further releases
            deathLogic.KilledWhenDeathDefiance();
            spawner.mindOnField--;
        }
    }

    private void ShootMindProj()
    {
        GameObject obj = Instantiate(mindProjectile, spawnPoint.transform.position, Quaternion.identity);
        AudioManager.instance.PlaySFX("mindattack");
        animator.SetTrigger("Attack");
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (player.gameObject.transform.position - spawnPoint.transform.position).normalized;
            rb.linearVelocity = dir * projectileSpeed;
        }
    }
}
