using UnityEngine;

public class BossAttack_TrackingMIssile : MonoBehaviour
{
    public PlayerHealth player;
    public Dashing playerDash;
    public GameObject missileObj;
    public Transform spawnPoint;
    GameObject missileHolder;
    SurgeLogic surgeLogic;
    public float damage;
    float speed = 5f;
    float explodeTime;
    float explodeTimer = 4f;
    bool spawned = false;

    private void Start()
    {
        explodeTime = explodeTimer;
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }

    private void Update()
    {
        if (spawned)
        {
            explodeTime -= Time.deltaTime;
            if (explodeTime <= 0)
            {
                ExplodeMissile(missileHolder);
            }
        }
        MissileHitPlayer();
    }
    public void ShootMissile()
    {
        GameObject obj = Instantiate(missileObj, spawnPoint.position, Quaternion.identity);
        missileHolder = obj;
        Rigidbody missilerb = missileHolder.GetComponent<Rigidbody>();
        missilerb.linearVelocity = missileHolder.transform.forward * speed;
        spawned = true;
    }

    private void ExplodeMissile(GameObject missileholder)
    {
        if (missileholder != null)
        {
            CheckCollision checkExplode = missileholder.GetComponent<CheckCollision>();
            if (checkExplode.collided)
            {
                checkExplode.HandleExplosion(player, playerDash, surgeLogic, damage);
                explodeTime = explodeTimer;
                spawned = false;
            }
        }
    }

    private void MissileHitPlayer()
    {
        if (missileHolder != null)
        {
            CheckCollision checkExplode = missileHolder.GetComponent<CheckCollision>();
            if (checkExplode.headOnCollide)
            {
                checkExplode.HandleContact(player, playerDash, surgeLogic, damage);
                spawned = false;
                explodeTime = explodeTimer;
            }
        }
    }
}
