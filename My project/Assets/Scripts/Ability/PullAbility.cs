using UnityEngine;
using UnityEngine.AI;

public class PullAbility : MonoBehaviour
{
    public float pullStrength;
    public LayerMask pullableLayer;
    public float stopDistance;
    PullAbilityObj pullAbilityObj;
    private void OnTriggerStay(Collider other)
    {
        //pullAbilityObj.pullOff = true;

        // Check if the object is on the pullable layer
        if (((1 << other.gameObject.layer) & pullableLayer) != 0)
        {
            // Get the NavMeshAgent of the object
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                // Calculate the direction to the pulling object
                Vector3 direction = transform.position - other.transform.position;

                // Stop pulling if the object is close enough
                if (direction.magnitude > stopDistance)
                {
                    // Adjust the agent's position manually (optional) or set its destination
                    Vector3 newPosition = Vector3.MoveTowards(
                        other.transform.position,
                        transform.position,
                        pullStrength * Time.deltaTime
                    );

                    // Set the agent's position or destination
                    agent.Warp(newPosition); // Move the agent directly
                }
                else
                {
                    // Stop the agent to prevent further movement
                    agent.isStopped = true;
                }
            }
        }
        //else
        //{
        //    pullAbilityObj.pullOff = false;
        //}
    }
}
