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
    bool coroutineRunning = false;
    float resetBool;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerHealth>();
        glitchCon = GameObject.Find("GameController").GetComponent<GlitchController>();
    }

    void Update()
    {
        resetBool += Time.deltaTime;


        if (!collided)
        {
            Destroy(gameObject, deactivTime);
        }
        if (resetBool >= 0.3f && coroutineRunning)
        {
            coroutineRunning = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
            return;

        if (other.CompareTag("PlayerObj"))
        {
            glitchCon.GlitchEffect();
            collided = true;
            player.TakeDamage(Unit.Damage);
            if (!coroutineRunning)
            {
                StartCoroutine(ResetMat());
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator ResetMat()
    {
        yield return new WaitForSeconds(0.3f);
        glitchCon.ResetGlitch();
        coroutineRunning = true;
    }
}
