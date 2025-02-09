using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltiLogic : BaseAbility
{
    [SerializeField]
    private LayerMask enemyLayer;
    public PlayerHealth health;
    HashSet<Collider> activeEnemies = new HashSet<Collider>();
    public GameObject panel;
    public TMP_Text cooldownText;
    public float dmgPerTick;
    public float timeBfrHeal;
    public float healRadius;
    public bool inUse = false;
    const float noEnemyCooldownTime = 1.5f;
    float enemyMultiplier = 0.6f;
    float damageMultiplier = 0.4f;
    float totalDamageDealt;
    float noEnemyTimer = 0f;
    float numberOfEnemies = 0f;
    bool noEnemiesDetected = false;
    bool activated = false;
    private void Start()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = cooldownTimer.ToString("F1");
            if (cooldownTimer <= 0f)
            {
                panel.SetActive(false);
                cooldownText.text = "";
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

        if (activated)
        {
            AudioManager.instance.PlaySFX("ulti");
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

        inUse = true;
        activated = true;
        noEnemyTimer = 0f;
        noEnemiesDetected = false;
        panel.SetActive(true);
    }
    private IEnumerator DmgNHeal(Health enemyHealth, Collider enemyCollider)
    {
        float elapsedTime = 0f;

        while (elapsedTime < timeBfrHeal)
        {
            float damageToDeal = enemyHealth.currentHealth * 0.08f + dmgPerTick;
            damageToDeal = Mathf.Max(damageToDeal, 1f);

            enemyHealth.TakeDamage(damageToDeal);
            totalDamageDealt += damageToDeal;

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
