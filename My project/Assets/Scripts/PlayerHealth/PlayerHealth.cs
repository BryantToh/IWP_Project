using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public OnField Unit;
    float currentHealth;
    float damage;
    float juggernautDamage;
    SentinelHealth sentinel;
    JuggernautHealth juggernaut;
    private float jHitCount = 0.0f;
    private float resetDamageTimer = 0.0f;
    PlayerMovement player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AttackCheck.checkEnemy && !damageSources.Contains(other) && player.kickSteps >= 0)
        {
            damageSources.Add(other);

            sentinel = other.GetComponent<SentinelHealth>();
            juggernaut = other.GetComponent<JuggernautHealth>();

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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
