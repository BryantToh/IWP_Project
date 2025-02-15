using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    [HideInInspector]
    public static bool checkEnemy = false;
    public ParticleSystem particle;
    PlayerHealth playerHealth;
    private void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(particle, other.ClosestPoint(transform.position), Quaternion.identity);
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
