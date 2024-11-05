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
        if (Input.GetKeyDown(KeyCode.Space))
            currentHealth = playerHealth.TakeDamage(currentHealth, 5);

    }

    public void DamageTaken()
    {
    }
}
