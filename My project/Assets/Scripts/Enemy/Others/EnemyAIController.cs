using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask Ground, playerobj;

    [Header("Attack")]
    public float timeBetweenAttacks;
    [HideInInspector]
    public bool alreadyAttacked;
    [HideInInspector]
    public bool playerInAttackRange;

    public float attackRange;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
            playerInAttackRange = true;
        else
            playerInAttackRange = false;

        if (playerInAttackRange)
        {
            Attacking();
        }
        else
        {
            Chasing();
        }
    }

    protected virtual void Chasing()
    {
        agent.SetDestination(player.position);
    }

    protected virtual void Attacking()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
