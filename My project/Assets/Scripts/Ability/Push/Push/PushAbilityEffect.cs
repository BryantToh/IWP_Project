using UnityEngine;

public class PushAbilityEffect : BaseAbility
{
    private float effectRadius = 6f;
    public LayerMask affectedLayer;
    [HideInInspector]
    public bool canAOE = false;
    private bool hasBeenUsed = false;
    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                hasBeenUsed = false;
            }
        }
    }
    public void ApplyPushback(Vector3 sourcePosition)
    {
        if (hasBeenUsed)
            return;

        Collider[] colliders = Physics.OverlapSphere(sourcePosition, effectRadius, affectedLayer);
        foreach (var collider in colliders)
        {
            PushAbilityLogic pushBack = collider.GetComponent<PushAbilityLogic>();
            if (pushBack != null)
            {
                pushBack.ApplyPush(sourcePosition);
            }
        }
        hasBeenUsed = true;
        canAOE = true;
    }
    public override void Activate()
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }
        ApplyPushback(transform.position);
    }
}
