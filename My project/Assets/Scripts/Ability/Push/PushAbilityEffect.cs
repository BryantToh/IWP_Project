using UnityEngine;

public class PushAbilityEffect : BaseAbility
{
    private float effectRadius = 6f;
    public LayerMask affectedLayer;

    public void ApplyPushback(Vector3 sourcePosition)
    {
        Collider[] colliders = Physics.OverlapSphere(sourcePosition, effectRadius, affectedLayer);

        foreach (var collider in colliders)
        {
            PushAbilityLogic pushBack = collider.GetComponent<PushAbilityLogic>();
            if (pushBack != null)
            {
                pushBack.ApplyPush(sourcePosition);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
    public override void Activate()
    {
        ApplyPushback(transform.position);
    }
}
