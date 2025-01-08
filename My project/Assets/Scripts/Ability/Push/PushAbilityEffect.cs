using UnityEngine;

public class PushAbilityEffect : BaseAbility
{
    private float effectRadius = 6f; // Range of the pushback
    public LayerMask affectedLayer; // Layer mask for affected objects

    public void ApplyPushback(Vector3 sourcePosition)
    {
        // Find all colliders within the radius
        Collider[] colliders = Physics.OverlapSphere(sourcePosition, effectRadius, affectedLayer);

        foreach (var collider in colliders)
        {
            // Check if the collider has a PushBackAgent component
            PushAbilityLogic pushBack = collider.GetComponent<PushAbilityLogic>();
            if (pushBack != null)
            {
                // Apply the pushback
                pushBack.ApplyPush(sourcePosition);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Visualize the effect radius in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
    public override void Activate()
    {
        ApplyPushback(transform.position);
    }
}
