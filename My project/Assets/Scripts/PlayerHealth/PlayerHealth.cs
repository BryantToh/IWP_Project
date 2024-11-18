using UnityEngine;

public class PlayerHealth : Health
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.LogWarning("Player takes damage");
            TakeDamage(Unit.Damage);
        }
    }
}
