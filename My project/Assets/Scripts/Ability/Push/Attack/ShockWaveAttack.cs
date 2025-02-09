using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveAttack : MonoBehaviour
{
    PushAbilityEffect pushAbilityEffect;
    [SerializeField]
    LayerMask enemyMask;
    private HashSet<Health> damagedEnemies = new HashSet<Health>();
    [SerializeField]
    float areaOfEffect = 0f;
    float dmgInterval = 1.3f;
    public float waveDamage = 0f;
    int waveCount = 0;
    void Start()
    {
        pushAbilityEffect = GetComponent<PushAbilityEffect>();
    }
    void Update()
    {
        if(pushAbilityEffect.canAOE)
        {
            AOEShockwave();
        }

        if (waveCount == 3)
        {
            ResetAbility();
        }
    }
    private void AOEShockwave()
    {
        if (waveCount >= 3) return; // Prevent extra shockwaves

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, areaOfEffect, enemyMask);
        foreach (var collider in enemyColliders)
        {
            Health enemyHealth = collider.GetComponent<Health>();
            if (enemyHealth != null && !damagedEnemies.Contains(enemyHealth))
            {
                damagedEnemies.Add(enemyHealth);
                StartCoroutine(AOEDamage(enemyHealth));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
    private void ResetAbility()
    {
        waveCount = 0;  // Reset wave count immediately
        damagedEnemies.Clear();  // Clear tracked enemies
        StopAllCoroutines(); // Stop any ongoing damage coroutines

        pushAbilityEffect.canAOE = false;
        pushAbilityEffect.isOnCooldown = true;
        pushAbilityEffect.cooldownTimer = pushAbilityEffect.cooldownTime;
    }


    private IEnumerator AOEDamage(Health enemyHealth)
    {
        yield return new WaitForSeconds(dmgInterval);

        if (waveCount >= 3) yield break; // Don't apply damage if max waves reached

        enemyHealth.TakeDamage(waveDamage);
        waveCount++;
        damagedEnemies.Remove(enemyHealth);

        if (waveCount == 3)
        {
            ResetAbility();
        }
    }

}
