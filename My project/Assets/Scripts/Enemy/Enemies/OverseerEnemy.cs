using UnityEngine;

public class OverseerEnemy : EnemyAIController
{
    Collider playerCol;
    OverseerHealth enemyHealth;
    public float rangedAttack;
    public float meleeAttack;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<OverseerHealth>();
        base.Start();
    }

    protected override void Update()
    {
        agent.stoppingDistance = attackRange;
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
