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
    float dmgInterval = 3f;
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

        if (waveCount == 5)
        {
            ResetAbility();
        }
    }
    private void AOEShockwave()
    {
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
        pushAbilityEffect.canAOE = false;
        waveCount = 0;
        damagedEnemies.Clear();
        pushAbilityEffect.isOnCooldown = true;
        pushAbilityEffect.cooldownTimer = pushAbilityEffect.cooldownTime;
    }

    private IEnumerator AOEDamage(Health enemyHealth)
    {
        yield return new WaitForSeconds(dmgInterval);
        enemyHealth.TakeDamage(waveDamage);
        waveCount++;
        damagedEnemies.Remove(enemyHealth);
    }
}
