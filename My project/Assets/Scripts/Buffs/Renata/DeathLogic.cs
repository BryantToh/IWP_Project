using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DeathLogic : BaseAbility
{
    public PlayerHealth playerHealth;
    float enemyKillCount = 0f;
    public bool activated = false;
    bool startDecr = false;
    bool healthReset = false;
    float timer = 0f;
    float totalDuration = 5f;
    float healthDecreaseRate = 0f;

    public override void Activate()
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }
        activated = true;
    }

    void Start()
    {
        timer = abilityDuration;
        enemyKillCount = 0f;
        healthDecreaseRate = playerHealth.Unit.Health / totalDuration;
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        if (activated)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ResetAbility();
                isOnCooldown = true;
            }
            if (playerHealth.currentHealth <= 0f)
            {
                startDecr = true;
            }
        }

        if (startDecr)
        {
            BuffEffect();
        }
    }

    void BuffEffect()
    {
        if (!healthReset)
        {
            playerHealth.currentHealth = playerHealth.Unit.Health;
            timer = abilityDuration;
            healthReset = true;
        }
        playerHealth.currentHealth -= healthDecreaseRate * Time.deltaTime;
        playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0f, playerHealth.Unit.Health);
        timer -= Time.deltaTime;

        if (timer <= 0f && enemyKillCount < 1)
        {
            Debug.LogWarning("player dies");
            playerHealth.gameObject.SetActive(false);
        }
        else if (timer > 0f && enemyKillCount >= 1)
        {
            Debug.LogWarning("player survives");
            playerHealth.currentHealth = playerHealth.Unit.Health * 0.7f;
            ResetAbility();
        }
    }

    void ResetAbility()
    {
        activated = false;
        timer = abilityDuration;
        cooldownTimer = cooldownTime;
        startDecr = false;
        healthReset = false;
    }

    public void KilledWhenDeathDefiance()
    {
        if (activated)
        {
            enemyKillCount++;
            Debug.LogWarning("enemy left" + enemyKillCount + " / 1");
        }
    }
}
