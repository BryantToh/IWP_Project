using UnityEngine;

public class ZoneSphereCol : MonoBehaviour
{
    float damage = 15;
    SurgeLogic surgeLogic;
    Dashing playerDash;
    private void Start()
    {
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
        playerDash = surgeLogic.GetComponentInParent<Dashing>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !playerDash.isDashing)
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
        }
        else if (playerDash.isDashing)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
        }
    }
}
