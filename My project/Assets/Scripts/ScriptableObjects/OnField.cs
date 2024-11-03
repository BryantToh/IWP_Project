using UnityEngine;

[CreateAssetMenu(fileName = "OnField")]
public class OnField : ScriptableObject
{
    public float Health;
    public float Damage;
    public float maxDashCount;

    public float TakeDamage(float currenthealth, float damage)
    {
        currenthealth -= damage;

        if (currenthealth <= 0)
        {
            Debug.LogWarning("dead");
        }
        else
        {
            Debug.LogWarning(currenthealth);
        }
        return currenthealth;
    }
}
