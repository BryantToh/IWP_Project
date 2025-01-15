using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public OnField Unit;
    PlayerHealth player;
    GlitchController glitchCon;
    public LayerMask enemyLayer;
    float deactivTime = 5f;
    bool collided = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerHealth>();
        glitchCon = GameObject.Find("GameController").GetComponent<GlitchController>();
    }

    void Update()
    {
        if (!collided)
        {
            Destroy(gameObject, deactivTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
            return;

        if (other.CompareTag("PlayerObj"))
        {
            glitchCon.GlitchEffect();
            glitchCon.resetChecking = true;
            collided = true;
            player.TakeDamage(Unit.Damage);
            Destroy(gameObject);
        }
    }
}
