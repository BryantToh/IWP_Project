using UnityEngine;

public class Health : MonoBehaviour
{
    public OnField Unit;
    //[HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public EnemySpawner spawner;
    [HideInInspector]
    public bool canDie = false;
    public bool isReleased = false;
    protected ObjectPooler pooler;
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
