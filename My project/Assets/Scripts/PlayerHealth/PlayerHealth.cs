using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public OnField Unit;
    public GlitchController glitchController;
    SentinelHealth sentinel;
    JuggernautHealth juggernaut;
    PhaseHealth phase;
    MindbreakersHealth mindbreakers;
    OverseerHealth overseer;
    PlayerMovement player;
    Coroutine healingCoroutine;
    [HideInInspector]
    public HashSet<Collider> damageSources = new HashSet<Collider>();
    [HideInInspector]
    public List<Collider> detectedColliders = new List<Collider>();
    [SerializeField]
    DeathLogic deathLogic;
    public Slider healthSlider;
    public float currentHealth;
    float damage;
    float juggernautDamage;
    private float jHitCount = 0.0f;
    private float resetDamageTimer = 0.0f;
    int hitCount = 0;
    private float checkTimer = 3f;
    private float timer = 0;
    private void Start()
    {
        damage = Unit.Damage;
        juggernautDamage = damage;
        currentHealth = Unit.Health;
        healthSlider.maxValue = Unit.Health;
        healthSlider.value = currentHealth;
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        resetDamageTimer += Time.deltaTime;

        if (resetDamageTimer >= 4.0f)
        {
            jHitCount = 0.0f;
            resetDamageTimer = 0.0f;
            juggernautDamage = damage;
        }
        if (hitCount > 0)
        {
            timer += Time.deltaTime;

            if (timer >= checkTimer)
            {
                glitchController.isResetting = true;
                timer = 0;
                hitCount = 0;
            }
        }

        if (glitchController.isResetting)
        {
            glitchController.ResetGlitch();
        }

        DealDmg();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MindProjectile"))
        {
            Destroy(other.gameObject);
            hitCount++;
            timer = 0;
            glitchController.GlitchEffect();
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value -= damage;
        AudioManager.instance.PlaySFX("hit");
        if (currentHealth <= 0 && !deathLogic.activated)
        {
            healthSlider.fillRect.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    public void Healing(float totalHealing)
    {
        float healingDuration = 3f;
        float healAmountPerSecond = totalHealing / healingDuration;
        if (healingCoroutine != null)
        {
            StopCoroutine(healingCoroutine);
        }
        healingCoroutine = StartCoroutine(HealOverTime(healAmountPerSecond, healingDuration));
    }

    private void DealDmg()
    {
        if (AttackCheck.checkEnemy && player.kickSteps > -1)
        {
            for (int i = detectedColliders.Count - 1; i >= 0; i--)
            {
                Collider other = detectedColliders[i];

                if (!damageSources.Contains(other))
                    return;

                SentinelHealth sentinel = other.GetComponentInParent<SentinelHealth>();
                JuggernautHealth juggernaut = other.GetComponentInParent<JuggernautHealth>();
                PhaseHealth phase = other.GetComponentInParent<PhaseHealth>();
                MindbreakersHealth mindbreakers = other.GetComponentInParent<MindbreakersHealth>();
                OverseerHealth overseer = other.GetComponentInParent<OverseerHealth>();

                float currentDamage = damage;

                if (player.chargeAttack)
                {
                    currentDamage *= 2f;
                }

                if (sentinel != null)
                {
                    sentinel.TakeDamage(currentDamage);
                    AudioManager.instance.PlaySFX("hit");
                }
                else if (juggernaut != null)
                {
                    if (jHitCount < 4)
                    {
                        juggernaut.TakeDamage(juggernautDamage * (player.chargeAttack ? 2f : 1f));
                        AudioManager.instance.PlaySFX("hit");
                        jHitCount++;
                    }
                    else if (jHitCount >= 4)
                    {
                        juggernautDamage += 2;
                        juggernaut.TakeDamage(juggernautDamage * (player.chargeAttack ? 2f : 1f));
                        AudioManager.instance.PlaySFX("hit");
                        jHitCount++;
                    }
                    resetDamageTimer = 0.0f;
                }
                else if (phase != null)
                {
                    phase.TakeDamage(currentDamage);
                    AudioManager.instance.PlaySFX("hit");
                }
                else if (mindbreakers != null)
                {
                    mindbreakers.TakeDamage(currentDamage);
                    AudioManager.instance.PlaySFX("hit");
                }
                else if (overseer != null)
                {
                    overseer.TakeDamage(currentDamage);
                    AudioManager.instance.PlaySFX("hit");
                }
                detectedColliders.RemoveAt(i);
                damageSources.Remove(other);
            }
        }
        AttackCheck.checkEnemy = false;
    }

    private IEnumerator HealOverTime(float healAmountPerSecond, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            currentHealth += healAmountPerSecond * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, currentHealth, Unit.Health);
            healthSlider.value = currentHealth;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
