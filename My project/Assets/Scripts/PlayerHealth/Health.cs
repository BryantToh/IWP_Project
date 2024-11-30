 using UnityEngine;

public class Health : MonoBehaviour
{
    public OnField Unit;
    public float currentHealth;

    protected virtual void Start()
    {
        currentHealth = Unit.Health;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.LogWarning("Dead");
            gameObject.SetActive(false);
        }
    }
}
