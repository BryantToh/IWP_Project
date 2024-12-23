using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    [HideInInspector]
    public static bool checkEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            checkEnemy = true;
        }
    }
}
