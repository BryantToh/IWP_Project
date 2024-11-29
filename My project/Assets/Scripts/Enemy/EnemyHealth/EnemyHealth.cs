using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    public float rotationSpd;
    float spinMany;
    float rotationAmount = 0f;
    Vector3 OgPos;
    SentinelEnemy sentinel;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        sentinel = GetComponent<SentinelEnemy>();
        base.Start();
    }

    public void AttackPlayer(Collider other)
    {
        if (!sentinel.playerInAttackRange)
            return;  
        
        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            StartCoroutine(attackCoroutine());
            player.TakeDamage(Unit.Damage);
        }
    }

    public void AttackReset(Collider other)
    {
        if (damageSources.Contains(other))
        {
            damageSources.Remove(other);
        }
    }
    private IEnumerator attackCoroutine()
    {
        rotationAmount = 0f; // Reset rotation amount for each attack.
        float duration = 0.8f; // Total time to complete the rotation.
        float elapsedTime = 0f;
        Vector3 startPos = transform.position; // Store the original position.

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor (0 to 1).
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // Smoothly interpolate the value.

            // Calculate the current rotation step.
            float currentRotationStep = Mathf.Lerp(0, 360f, t);
            float rotationStep = currentRotationStep - rotationAmount;

            // Apply the rotation step.
            transform.rotation *= Quaternion.Euler(0, rotationStep, 0);

            // Calculate vertical movement using a sine wave for smooth up-and-down motion.
            float verticalOffset = Mathf.Sin(t * Mathf.PI) * 0.8f;

            // Apply the position change.
            transform.position = new Vector3(startPos.x, startPos.y + verticalOffset, startPos.z);
            rotationAmount = currentRotationStep;
            yield return null;
        }

        // Ensure the final rotation and position are reset correctly.
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (360f - rotationAmount), transform.eulerAngles.z);
        transform.position = startPos;
    }

}
