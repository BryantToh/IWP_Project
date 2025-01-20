using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    [HideInInspector]
    public static bool checkEnemy = false;
    PlayerHealth playerHealth;
    private void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            checkEnemy = true;
            playerHealth.detectedColliders.Add(other);
            playerHealth.damageSources.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        checkEnemy = false;
    }
}
