using UnityEngine;

public class Health : MonoBehaviour
{
    public OnField Unit;
    protected float currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = Unit.Health;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //Debug.LogWarning("Dead");
            Destroy(gameObject);
        }
        else
        {
            //Debug.LogWarning($"Current Health: {currentHealth}");
        }
    }
}
