using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public OnField Unit;
    PlayerHealth player;
    SurgeLogic surgeLogic;
    Dashing playerDash;
    GlitchController glitchCon;
    public LayerMask enemyLayer;
    float deactivTime = 5f;
    float dist;
    bool collided = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponent<PlayerHealth>();
        playerDash = player.GetComponent<Dashing>();
        glitchCon = GameObject.Find("GameController").GetComponent<GlitchController>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }


    void Update()
    {
        if (!collided)
        {
            Destroy(gameObject, deactivTime);
        }
        dist = Vector3.Distance(transform.position, player.transform.position);
        distCheck();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
            return;


        if (other.CompareTag("PlayerObj") && !playerDash.isDashing)
        {
            glitchCon.GlitchEffect();
            glitchCon.resetChecking = true;
            collided = true;
            player.TakeDamage(Unit.Damage);
            Destroy(gameObject);
        }
        else if(distCheck() || other.CompareTag("PlayerObj") && playerDash.isDashing)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
            else
                Debug.Log("passive already active");
        }
    }

    bool distCheck()
    {
        if (dist <= 3f && playerDash.isDashing)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
            else
                Debug.Log("passive already active");
            return true;
        }
        else return false;
    }
}
