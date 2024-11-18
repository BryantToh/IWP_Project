using UnityEngine;

public class testdamage : Health
{
    PlayerHealth health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerHealth>();
        base.Start();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Parts"))
    //    {
    //        health.TakeDamage(Unit.Damage);
    //    }
    //}
}
