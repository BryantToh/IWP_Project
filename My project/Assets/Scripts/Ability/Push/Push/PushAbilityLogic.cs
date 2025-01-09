using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PushAbilityLogic : MonoBehaviour
{
    public float pushForce;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void ApplyPush(Vector3 sourcePosition)
    {
        if (agent == null) return;

        Vector3 pushDirection = (agent.transform.position - sourcePosition).normalized;

        Vector3 pushPosition = agent.transform.position + pushDirection * pushForce;

        agent.Warp(pushPosition);

        StartCoroutine(ResumeAgentAfterDelay(0.5f));
    }

    private IEnumerator ResumeAgentAfterDelay(float delay)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(delay);
        agent.isStopped = false;
    }
}
