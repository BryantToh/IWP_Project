using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class JuggernautHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    Dashing playerDash;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    public Animator animator;
    JuggernautEnemy juggernaut;
    DeathLogic deathLogic;
    SurgeLogic surgeLogic;
    public Slider healthSlider;
    public ParticleSystem particle;
    protected override void Start()
    {
        base.Start();
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
    }

    public void OnEnemySpawn()
    {
        pooler = ObjectPooler.Instance;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        playerDash = player.GetComponent<Dashing>();
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
            AudioManager.instance.PlaySFX("enemymelee");
            animator.SetTrigger("Attack");
            damageSources.Add(other);
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= juggernaut.attackRange && !playerDash.isDashing)
        {
            Collider playerColPos = player.GetComponent<Collider>();
            player.TakeDamage(Unit.Damage);
            Vector3 newPos = new Vector3(playerColPos.transform.position.x, playerColPos.transform.position.y + 1f, playerColPos.transform.position.z);
            Instantiate(particle, newPos, Quaternion.identity);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= juggernaut.attackRange && playerDash.isDashing ||
            Vector3.Distance(player.transform.position, transform.position) > juggernaut.attackRange && playerDash.isDashing)
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
        healthSlider.value -= damage;
        if (canDie && !isReleased) // Ensure release is only called once
        {
            ObjectPooler.Instance.Release("juggernaut", this);
            isReleased = true; // Set to true to prevent further releases
            deathLogic.KilledWhenDeathDefiance();
            //spawner.juggernautOnField--;
        }
    }

    public void OnGet()
    {
        currentHealth = Unit.Health;
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
        isReleased = false;
        canDie = false;
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        //pooler.isPooled = false;
        gameObject.SetActive(false);
        spawner.OnEnemyDefeated();
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
