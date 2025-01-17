using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    GameObject player;
    Rigidbody rb, playerrb;
    Vector3 _standardPrediction, _deviatedPrediction;
    public bool collided = false;
    public bool headOnCollide = false;
    public float _maxDistancePredict;
    public float _minDistancePredict;
    public float _maxTimePrediction;
    public float _deviationAmount;
    public float _deviationSpeed;
    float speed = 5;
    float rotateSpeed = 300;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj");
        rb = GetComponent<Rigidbody>();
        playerrb = player.GetComponent<Rigidbody>();
    }

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

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * speed;
        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, player.transform.position));
        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
    }

    public void HandleExplosion(PlayerHealth playerHealth, Dashing playerDash, SurgeLogic surgeLogic, float damage)
    {
        if (!playerDash.isDashing)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float maxDamage = damage;
            float minDamage = damage / 2;
            float maxDistance = 3.5f;
            float scaledDamage = Mathf.Lerp(maxDamage, minDamage, Mathf.Clamp01(distance / maxDistance));
            playerHealth.TakeDamage(scaledDamage);
        }
        else if (playerDash.isDashing)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
        }
        DestroyMissile();
    }

    public void HandleContact(PlayerHealth playerHealth, Dashing playerDash, SurgeLogic surgeLogic, float damage)
    {
        if (!playerDash.isDashing)
        {
            playerHealth.TakeDamage(damage);
        }
        else if (playerDash.isDashing)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
        }
        DestroyMissile();
    }

    public void DestroyMissile()
    {
        collided = false;
        headOnCollide = false;
        Destroy(gameObject);
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
        _standardPrediction = player.transform.position + playerrb.linearVelocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;
        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }
}
