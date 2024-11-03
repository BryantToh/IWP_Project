using UnityEngine;

public class testdamage : MonoBehaviour
{
    PlayerHealth health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            health.DamageTaken();
        }
    }
}
