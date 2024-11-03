using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public OnField playerHealth;
    private float currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = playerHealth.Health;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DamageTaken()
    {
       currentHealth = playerHealth.TakeDamage(currentHealth, 5);
    }
}
