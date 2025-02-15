using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class SentinelHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    Dashing playerDash;
    public Animator spinAnim;
    SentinelEnemy sentinel;
    DeathLogic deathLogic;
    SurgeLogic surgeLogic;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    public ParticleSystem attackNotice, particle;
    public Slider healthSlider;
    public float rotationSpd;
    float rotationAmount;
    protected override void Start()
    {
        base.Start();
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
    }
    public void OnEnemySpawn()
    {
        pooler = ObjectPooler.Instance;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerHealth>();
        playerDash = player.GetComponent<Dashing>();
        deathLogic = GameObject.FindGameObjectWithTag("deathdefi").GetComponent<DeathLogic>();
        sentinel = GetComponent<SentinelEnemy>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }
    public void AttackPlayer(Collider other)
    {
        if (!sentinel.playerInAttackRange)
            return;  
        
        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            spinAnim.SetTrigger("Attack");
            AudioManager.instance.PlaySFX("enemymelee");
            attackNotice.Play();
            StartCoroutine(attackCoroutine());
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= sentinel.attackRange && !playerDash.isDashing)
        {
            Collider playerColPos = player.GetComponent<Collider>();
            player.TakeDamage(Unit.Damage);
            Instantiate(particle, playerColPos.transform.position, Quaternion.identity);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) <= sentinel.attackRange && playerDash.isDashing || 
            Vector3.Distance(player.transform.position, transform.position) > sentinel.attackRange && playerDash.isDashing)
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
    private IEnumerator attackCoroutine()
    {
        rotationAmount = 0f;
        float duration = 0.8f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // Smoothly interpolate the value.

            // Calculate the current rotation step.
            float currentRotationStep = Mathf.Lerp(0, 360f, t);
            float rotationStep = currentRotationStep - rotationAmount;

            // Apply the rotation step.
            transform.rotation *= Quaternion.Euler(0, rotationStep, 0);

            // Calculate vertical movement using a sine wave for smooth up-and-down motion.
            //float verticalOffset = Mathf.Sin(t * Mathf.PI) * 0.8f;
            // Apply the position change.
            //transform.position = new Vector3(startPos.x, startPos.y + verticalOffset, startPos.z);
            rotationAmount = currentRotationStep;
            yield return null;
        }

        // Ensure the final rotation and position are reset correctly.
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (360f - rotationAmount), transform.eulerAngles.z);
        //transform.position = startPos;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthSlider.value -= damage;
        AudioManager.instance.PlaySFX("hit");
        if (canDie && !isReleased) // Ensure release is only called once
        {
            ObjectPooler.Instance.Release("sentinel", this);
            isReleased = true; // Set to true to prevent further releases
            deathLogic.KilledWhenDeathDefiance();
            spawner.sentinelOnField--;
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
        spinAnim.StopPlayback();
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
