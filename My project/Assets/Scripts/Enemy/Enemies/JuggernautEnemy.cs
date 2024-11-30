using UnityEngine;

public class JuggernautEnemy : EnemyAIController
{
    Collider playerCol;
    JuggernautHealth enemyHealth;
    protected override void Awake()
    {
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<JuggernautHealth>();
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
