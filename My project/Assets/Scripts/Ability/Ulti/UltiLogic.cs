using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltiLogic : BaseAbility
{
    [SerializeField]
    private LayerMask enemyLayer;
    public PlayerHealth health;
    HashSet<Collider> activeEnemies = new HashSet<Collider>();
    public float dmgPerTick;
    public float timeBfrHeal;
    public float healRadius;
    float enemyMultiplier = 0.4f;
    float damageMultiplier = 0.2f;
    float totalDamageDealt;
    bool activated = false;
    float noEnemyTimer = 0f;
    const float noEnemyCooldownTime = 2f;
    bool noEnemiesDetected = false;
    float numberOfEnemies = 0f;
    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
        if (activated && noEnemiesDetected)
        {
            noEnemyTimer += Time.deltaTime;
            if (noEnemyTimer >= noEnemyCooldownTime)
            {
                StartCooldown();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!activated)
            return;

        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            noEnemiesDetected = false;
            noEnemyTimer = 0f;

            if (!activeEnemies.Contains(other))
            {
                Health enemyHealth = other.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    activeEnemies.Add(other);
                    StartCoroutine(DmgNHeal(enemyHealth, other));
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (activeEnemies.Contains(other))
        {
            activeEnemies.Remove(other);
        }
        if (activeEnemies.Count == 0)
        {
            noEnemiesDetected = true;
        }
    }
    private int GetNumberOfEnemiesInRange()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, healRadius, enemyLayer);
        return enemyColliders.Length;
    }
    private float HealCalc()
    {
        numberOfEnemies = GetNumberOfEnemiesInRange();
        float healingAmount = (numberOfEnemies * enemyMultiplier) + (totalDamageDealt * damageMultiplier);
        return healingAmount;
    }
    private void StartCooldown()
    {
        activated = false;
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        noEnemyTimer = 0f;
        Debug.Log("Ability went on cooldown due to no enemies in range.");
    }
    public override void Activate()
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        activated = true;
        noEnemyTimer = 0f;
        noEnemiesDetected = false;
    }
    private IEnumerator DmgNHeal(Health enemyHealth, Collider enemyCollider)
    {
        float elapsedTime = 0f;
        while (elapsedTime < timeBfrHeal)
        {
            enemyHealth.TakeDamage(dmgPerTick);
            totalDamageDealt += dmgPerTick;
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

        health.Healing(HealCalc());
        activeEnemies.Remove(enemyCollider);
        totalDamageDealt = 0f;
        numberOfEnemies = 0f;
        if (activeEnemies.Count == 0)
        {
            noEnemiesDetected = true;
        }
    }
}
