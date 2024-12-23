using UnityEngine;
using UnityEngine.AI;

public class SentinelEnemy : EnemyAIController
{
    SentinelHealth enemyHealth;
    Collider playerCol;
    protected override void Awake()
    {
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<SentinelHealth>();
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Chasing()
    {
        base.Chasing();
    }

    protected override void Attacking()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            enemyHealth.AttackPlayer(playerCol);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected override void ResetAttack()
    {
        base.ResetAttack();
        enemyHealth.AttackReset(playerCol);
    }
}
