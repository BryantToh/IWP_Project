using UnityEngine;
using UnityEngine.AI;
public class PullAbilityLogic : MonoBehaviour
{
    public LayerMask pullableLayer;
    NavMeshAgent agent = null;
    PullAbilityObj pullAbilityObj;
    public float stopDistance;
    public float pullStrength;
    bool canDelete = false;
    public bool inUse = false;
    private void Start()
    {
        pullAbilityObj = GameObject.FindGameObjectWithTag("Abilityholder").GetComponent<PullAbilityObj>();
        inUse = true;
    }
    private void Update()
    {
        pullAbilityObj.abilityDuration += Time.deltaTime;

        if (pullAbilityObj.abilityDuration >= 5f)
        {
            Reset();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & pullableLayer) != 0 && !canDelete)
        {
            pullAbilityObj.pullOff = true;
            agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                Vector3 direction = transform.position - other.transform.position;

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
    }

    private void Reset()
    {
        canDelete = true;
        pullAbilityObj.pullOff = false;
        agent.isStopped = false;
        pullAbilityObj.isOnCooldown = true;
        pullAbilityObj.cooldownTimer = pullAbilityObj.cooldownTime;
        Destroy(gameObject);
    }
}
