using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    Dashing playerDash;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    public Animator animator;
    PhaseEnemy phase;
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
            animator.SetTrigger("Attack");
            AudioManager.instance.PlaySFX("enemymelee");
            damageSources.Add(other);
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= phase.attackRange && !playerDash.isDashing)
        {
            Collider playerColPos = player.GetComponent<Collider>();
            player.TakeDamage(Unit.Damage);
            Vector3 newPos = new Vector3(playerColPos.transform.position.x, playerColPos.transform.position.y + 0.5f, playerColPos.transform.position.z);
            Instantiate(particle, newPos, Quaternion.identity);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= phase.attackRange && playerDash.isDashing ||
            Vector3.Distance(player.transform.position, transform.position) > phase.attackRange && playerDash.isDashing)
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
            ObjectPooler.Instance.Release("phase", this);
            isReleased = true; // Set to true to prevent further releases
            deathLogic.KilledWhenDeathDefiance();
            //spawner.phaseOnField--;
        }
    }

    public void OnGet()
    {
        currentHealth = Unit.Health;
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
        //pooler.isPooled = true;
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
