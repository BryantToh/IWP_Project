using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PushAbilityLogic : MonoBehaviour
{
    public float pushForce; // The strength of the push
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void ApplyPush(Vector3 sourcePosition)
    {
        if (agent == null) return;
        // Calculate the direction from the source of the pushback
        Vector3 pushDirection = (agent.transform.position - sourcePosition).normalized;

        // Calculate the new position based on the force
        Vector3 pushPosition = agent.transform.position + pushDirection * pushForce;

        // Use NavMeshAgent.Warp for an instant position change
        agent.Warp(pushPosition);

        // Optionally, stop the agent briefly and then resume
        StartCoroutine(ResumeAgentAfterDelay(0.5f)); // Small delay before resuming movement
    }

    private IEnumerator ResumeAgentAfterDelay(float delay)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(delay);
        agent.isStopped = false;
    }
}
