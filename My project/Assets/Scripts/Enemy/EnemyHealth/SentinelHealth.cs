using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SentinelHealth : Health, IPooledEnemy
{
    PlayerHealth player;
    private HashSet<Collider> damageSources = new HashSet<Collider>();
    public float rotationSpd;
    float rotationAmount;
    public Animation spinAnim;
    SentinelEnemy sentinel;
    SurgeLogic surgeLogic;
    protected override void Start()
    {
        base.Start();
    }
    private void SetAnimProperty()
    {
        AnimationCurve curve = AnimationUtility.GetEditorCurve(spinAnim.clip,
        new EditorCurveBinding
        {
            path = "",
            type = typeof(Transform),
            propertyName = "Position.x"
        });
        if (curve != null)
        {
            // Modify the keyframes in the curve.
            Keyframe[] keyframes = curve.keys;
            for (int i = 0; i < keyframes.Length; i++)
            {
                keyframes[i].value = transform.position.x; // Example: Offset the value by 1.
            }
            // Apply the modified keyframes back to the curve.
            curve.keys = keyframes;

            // Update the animation clip with the modified curve.
            AnimationUtility.SetEditorCurve(spinAnim.clip,
                new EditorCurveBinding
                {
                    path = "",
                    type = typeof(Transform),
                    propertyName = "Position.x"
                },
                curve);

            Debug.Log("Animation clip property modified.");
        }
        else
        {
            Debug.LogWarning("Curve not found.");
        }
    }
    public void OnEnemySpawn()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<PlayerHealth>();
        sentinel = GetComponent<SentinelEnemy>();
        surgeLogic = GameObject.FindGameObjectWithTag("Surge").GetComponent<SurgeLogic>();
    }
    public void AttackPlayer(Collider other)
    {
        if (!sentinel.playerInAttackRange)
            return;  
        
        if (!damageSources.Contains(other))
        {
            damageSources.Add(other);
            StartCoroutine(attackCoroutine());
        }
    }
    public void AttackPlayerEvent()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= sentinel.attackRange)
        {
            player.TakeDamage(Unit.Damage);
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > sentinel.attackRange)
        {
            if (!surgeLogic.attackDodged)
                surgeLogic.attackDodged = true;
            else
                Debug.Log("passive already active");
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
        rotationAmount = 0f;
        float duration = 0.8f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // Smoothly interpolate the value.

            // Calculate the current rotation step.
            float currentRotationStep = Mathf.Lerp(0, 360f, t);
            float rotationStep = currentRotationStep - rotationAmount;

            // Apply the rotation step.
            transform.rotation *= Quaternion.Euler(0, rotationStep, 0);

            // Calculate vertical movement using a sine wave for smooth up-and-down motion.
            //float verticalOffset = Mathf.Sin(t * Mathf.PI) * 0.8f;
            // Apply the position change.
            spinAnim.Play();
            //transform.position = new Vector3(startPos.x, startPos.y + verticalOffset, startPos.z);
            rotationAmount = currentRotationStep;
            yield return null;
        }

        // Ensure the final rotation and position are reset correctly.
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (360f - rotationAmount), transform.eulerAngles.z);
        //transform.position = startPos;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (canDie)
        {
            spawner.sentinelOnField--;
            ObjectPooler.Instance.Release("sentinel", this);
        }
    }

    public void OnGet()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public void OnDestroyInterface()
    {
        Destroy(gameObject);
    }

    public void SetPosNRot(Vector3 Pos, Quaternion Rot)
    {
        transform.position = Pos;
        transform.rotation = Rot;
    }
}
