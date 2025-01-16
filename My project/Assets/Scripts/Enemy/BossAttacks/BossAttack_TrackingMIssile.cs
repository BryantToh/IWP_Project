using UnityEngine;

public class BossAttack_TrackingMIssile : MonoBehaviour
{
    public PlayerHealth player;
    public Dashing playerDash;
    public GameObject missileObj;
    GameObject missileHolder;
    public Transform spawnPoint;
    SurgeLogic surgeLogic;
    public float damage;
    float explodeTime;
    float explodeTimer = 5f;
    float speed = 5f;
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
                ExplodeMissile();
                explodeTime = explodeTimer;
                spawned = false;
            }
        }

        MissileHitPlayer();
    }

    public void ShootMissile()
    {
        if (!spawned)
        {
            GameObject obj = Instantiate(missileObj, spawnPoint.position, Quaternion.Euler(90, 0, 0));
            missileHolder = obj;
            missileHolder.transform.LookAt(player.transform);
            Rigidbody missileRb = missileHolder.GetComponent<Rigidbody>();
            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
            Vector3 direction = (newPos - spawnPoint.position).normalized;
            missileRb.linearVelocity = direction * speed;
            spawned = true;
        }
    }

    public void ExplodeMissile()
    {
        if (missileHolder != null)
        {
            CheckCollision checkExplode = missileHolder.GetComponent<CheckCollision>();
            if (checkExplode.collided && !playerDash.isDashing)
            {
                DamageOnPosition();
            }
            else if (checkExplode.collided && playerDash.isDashing)
            {
                if (!surgeLogic.attackDodged)
                    surgeLogic.attackDodged = true;
            }
            Destroy(missileHolder);
            checkExplode.collided = false;
        }
    }

    public void MissileHitPlayer()
    {
        if (missileHolder != null)
        {
            CheckCollision checkExplode = missileHolder.GetComponent<CheckCollision>();
            if (!checkExplode.headOnCollide)
                return;

            if (checkExplode.headOnCollide && !playerDash.isDashing)
            {
                player.TakeDamage(damage);
            }
            else if (checkExplode.headOnCollide && playerDash.isDashing)
            {
                if (!surgeLogic.attackDodged)
                    surgeLogic.attackDodged = true;
            }
            Destroy(missileHolder);
            checkExplode.headOnCollide = false;
        }
    }

    private void DamageOnPosition()
    {
        if (missileHolder != null)
        {
            float distance = Vector3.Distance(player.transform.position, missileHolder.transform.position);
            float maxDamage = damage;
            float minDamage = damage / 2; 
            float maxDistance = 3.5f;
            float scaledDamage = Mathf.Lerp(maxDamage, minDamage, Mathf.Clamp01(distance / maxDistance));
            player.TakeDamage(scaledDamage);

            Debug.Log($"Damage dealt: {scaledDamage}, Distance: {distance}");
        }
    }
}
