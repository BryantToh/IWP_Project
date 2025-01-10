using UnityEngine;

public class Health : MonoBehaviour
{
    public OnField Unit;
    public float currentHealth;
    [HideInInspector]
    public EnemySpawner spawner;
    [HideInInspector]
    protected bool canDie = false;
    protected virtual void Start()
    {
        currentHealth = Unit.Health;
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            canDie = true;
        }
    }
}
