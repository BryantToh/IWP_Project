using System.Collections;
using UnityEngine;

public class UltiLogic : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyLayer;
    public float dmgPerTick;
    public float timeBfrHeal;
    public PlayerHealth health;
    public float healRadius; // Radius to count enemies
    private float enemyMultiplier; // Healing per enemy
    private float damageMultiplier; // Percentage of total damage dealt as healing
    private float totalDamageDealt;
    public bool activated = false;
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                StartCoroutine(DmgNHeal(enemyHealth));
            }
        }
    }

    private int GetNumberOfEnemiesInRange()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, healRadius, enemyLayer);
        return enemyColliders.Length;
    }

    private float HealCalc()
    {
        int numberOfEnemies = GetNumberOfEnemiesInRange();
        float healingAmount = (numberOfEnemies * enemyMultiplier) + (totalDamageDealt * damageMultiplier);
        return healingAmount;
    }

    private IEnumerator DmgNHeal(Health enemyHealth)
    {
        enemyHealth.TakeDamage(dmgPerTick);
        totalDamageDealt += dmgPerTick; // Accumulate total damage dealt
        yield return new WaitForSeconds(timeBfrHeal);
        //health.Healing(HealCalc());
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the heal radius in the scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }
}
