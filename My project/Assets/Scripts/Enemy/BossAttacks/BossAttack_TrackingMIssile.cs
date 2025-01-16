using UnityEngine;

public class BossAttack_TrackingMIssile : MonoBehaviour
{
    public PlayerHealth player;
    public Dashing playerDash;
    public GameObject missileObj;
    public Transform spawnPoint;
    Vector3 _standardPrediction, _deviatedPrediction;
    GameObject missileHolder;
    SurgeLogic surgeLogic;
    public float damage;
    public float _maxDistancePredict = 100;
    public float _minDistancePredict = 5;
    public float _maxTimePrediction = 5;
    public float _deviationAmount = 10;
    public float _deviationSpeed = 2;
    float explodeTime;
    float explodeTimer = 5f;
    float speed = 5f;
    float rotateSpeed = 50;
    bool spawned = false;

    private void Start()
    {
        explodeTime = explodeTimer;
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }

    private void Update()
    {
        if (spawned && missileHolder != null)
        {
            explodeTime -= Time.deltaTime;
            if (explodeTime <= 0)
            {
                ExplodeMissile();
                explodeTime = explodeTimer;
            }
        }
        MissileHitPlayer();
    }

    private void FixedUpdate()
    {
        if (missileHolder != null)
        {
            Rigidbody missileRb = missileHolder.GetComponent<Rigidbody>();

            // Predict player movement and adjust target
            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(missileHolder.transform.position, player.transform.position));
            PredictMovement(leadTimePercentage);
            AddDeviation(leadTimePercentage);

            // Adjust missile rotation toward the predicted position
            RotateRocket();

            // Update missile velocity to maintain speed in its forward direction
            missileRb.linearVelocity = missileHolder.transform.forward * speed;
        }
    }


    public void ShootMissile()
    {
        if (!spawned)
        {
            // Instantiate the missile
            GameObject obj = Instantiate(missileObj, spawnPoint.position, Quaternion.identity);

            // Assign the missile to the holder
            missileHolder = obj;

            // Calculate initial direction toward the player
            Vector3 initialDirection = (player.transform.position - spawnPoint.position).normalized;

            // Set the missile's forward direction
            missileHolder.transform.rotation = Quaternion.LookRotation(initialDirection);

            // Apply initial velocity
            Rigidbody missileRb = missileHolder.GetComponent<Rigidbody>();
            missileRb.linearVelocity = initialDirection * speed;

            // Mark the missile as spawned
            spawned = true;
        }
    }


    public void ExplodeMissile()
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
        spawned = false;
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
            spawned = false;
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
        }
    }

    private void PredictMovement(float leadTimePercentage)
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = playerRb.position + playerRb.linearVelocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = missileHolder.transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        Rigidbody missileRb = missileHolder.GetComponent<Rigidbody>();
        var heading = _deviatedPrediction - missileHolder.transform.position;

        var rotation = Quaternion.LookRotation(heading);
        missileRb.MoveRotation(Quaternion.RotateTowards(missileHolder.transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }
}
