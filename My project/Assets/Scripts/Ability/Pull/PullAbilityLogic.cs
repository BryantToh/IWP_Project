using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PullAbilityLogic : MonoBehaviour
{
    public LayerMask pullableLayer;
    //NavMeshAgent agent = null;
    PullAbilityObj pullAbilityObj;
    public float stopDistance;
    public float pullStrength;
    bool canDelete = false;

    private List<NavMeshAgent> agentsStored = new List<NavMeshAgent>();
    private void Start()
    {
        pullAbilityObj = GameObject.FindGameObjectWithTag("Abilityholder").GetComponent<PullAbilityObj>();
        pullAbilityObj.inUse = true;
    }
    private void Update()
    {
        pullAbilityObj.abilityDuration += Time.deltaTime;

        if (pullAbilityObj.abilityDuration >= 5f)
        {
            Reset();
        }

        // loop through stored agents
        foreach (NavMeshAgent agent in agentsStored)
        {
            Vector3 direction = transform.position - agent.transform.position;

            if (direction.magnitude > stopDistance)
            {
                //Vector3 newPosition = Vector3.MoveTowards(
                //    other.transform.position,
                //    transform.position,
                //    pullStrength * Time.deltaTime
                //);
                agent.SetDestination(transform.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // find any agent that entered trigger and store in agentsStored
        if (((1 << other.gameObject.layer) & pullableLayer) != 0 && !canDelete)
        {
            pullAbilityObj.pullOff = true;
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agentsStored.Add(agent);
              
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & pullableLayer) != 0 && !canDelete)
        {
            pullAbilityObj.pullOff = true;
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agentsStored.Remove(agent);
            }
        }
    }
    private void Reset()
    {
        canDelete = true;
        pullAbilityObj.pullOff = false;

        foreach (NavMeshAgent agent in agentsStored)
        {
            agent.isStopped = false;
        }
        agentsStored = new List<NavMeshAgent>();
        pullAbilityObj.isOnCooldown = true;
        pullAbilityObj.cooldownTimer = pullAbilityObj.cooldownTime;
        Destroy(gameObject);
    }
}
