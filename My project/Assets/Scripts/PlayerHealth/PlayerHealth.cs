using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public OnField Unit;
    public GlitchController glitchController;
    SentinelHealth sentinel;
    JuggernautHealth juggernaut;
    PhaseHealth phase;
    MindbreakersHealth mindbreakers;
    PlayerMovement player;
    Coroutine healingCoroutine;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    [SerializeField]
    DeathLogic deathLogic;
    [HideInInspector]
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

        if (currentHealth <= 0f)
        {
            gameObject.SetActive(false);
        }

        if (glitchController.isResetting)
        {
            glitchController.ResetGlitch();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AttackCheck.checkEnemy && !damageSources.Contains(other) && player.kickSteps >= 0)
        {
            damageSources.Add(other);

            sentinel = other.GetComponent<SentinelHealth>();
            juggernaut = other.GetComponent<JuggernautHealth>();
            phase = other.GetComponent<PhaseHealth>();
            mindbreakers = other.GetComponent<MindbreakersHealth>();

            if (sentinel != null)
            {
                sentinel.TakeDamage(damage);
            }
            else if (juggernaut != null)
            {
                if (jHitCount < 4)
                {
                    juggernaut.TakeDamage(juggernautDamage);
                    jHitCount++;
                }
                else if (jHitCount >= 4)
                {
                    juggernautDamage += 2;
                    juggernaut.TakeDamage(juggernautDamage);
                    jHitCount++;
                }
                resetDamageTimer = 0.0f;
            }
            else if (phase != null)
            {
                phase.TakeDamage(damage);
            }
            else if (mindbreakers != null)
            {
                mindbreakers.TakeDamage(damage);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (damageSources.Contains(other))
        {
            damageSources.Remove(other);
        }
        AttackCheck.checkEnemy = false;
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
        if (currentHealth <= 0 && !deathLogic.activated)
        {
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

    private IEnumerator HealOverTime(float healAmountPerSecond, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            currentHealth += healAmountPerSecond * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, currentHealth, Unit.Health);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
