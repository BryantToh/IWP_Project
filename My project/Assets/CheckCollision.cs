using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public bool collided = false;
    public bool headOnCollide = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            collided = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerObj"))
        {
            headOnCollide = true;
        }
    }
}
