using UnityEngine;

public class MindBreakersEnemy : EnemyAIController
{
    Collider playerCol;
    MindbreakersHealth enemyHealth;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<MindbreakersHealth>();
        base.Start();
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