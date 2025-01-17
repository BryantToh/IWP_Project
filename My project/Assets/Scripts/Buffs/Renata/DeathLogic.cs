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
    float buffTimer = 0f;
    float buffDuration = 5f;

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
        ResetAbility();
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

        if (activated && !startDecr)
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
                buffTimer = buffDuration;
                timer = 1f;
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
        buffTimer -= Time.deltaTime;

        if (buffTimer <= 0f && enemyKillCount < 1)
        {
            Debug.LogWarning("Player dies");
            playerHealth.gameObject.SetActive(false);
        }
        else if (buffTimer < 0 && enemyKillCount >= 1)
        {
            Debug.LogWarning("Player survives");
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
        buffTimer = 0f;
    }

    public void KilledWhenDeathDefiance()
    {
        if (activated)
        {
            enemyKillCount++;
            Debug.LogWarning("Enemy left " + enemyKillCount + " / 1");
        }
    }
}
