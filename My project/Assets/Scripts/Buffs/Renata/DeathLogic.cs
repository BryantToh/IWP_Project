using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathLogic : BaseAbility
{
    public PlayerHealth playerHealth;
    public GameObject panel;
    public TMP_Text cooldownText;
    public Slider healthSlider;
    float enemyKillCount = 0f;
    public bool activated = false;
    public bool isUse = false;
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
        AudioManager.instance.PlaySFX("death");
        activated = true;
        panel.SetActive(true);
    }

    void Start()
    {
        ResetAbility();
        enemyKillCount = 0f;
        panel.SetActive(false);
        healthDecreaseRate = playerHealth.Unit.Health / totalDuration;
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = cooldownTimer.ToString("F1");
            if (cooldownTimer <= 0f)
            {
                cooldownText.text = "";
                isOnCooldown = false;
                panel.SetActive(false);
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
            isUse = true;
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
            healthSlider.value = playerHealth.currentHealth;
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
        AudioManager.instance.StopSFX("death");
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
