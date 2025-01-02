using UnityEngine;

public class MindBreakersEnemy : EnemyAIController
{
    Collider playerCol;
    MindbreakersHealth enemyHealth;
    public AbilityController abilityController;
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        abilityController = GameObject.FindGameObjectWithTag("PlayerObj").GetComponentInChildren<AbilityController>();
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<MindbreakersHealth>();
        base.Start();
    }

    protected override void Update()
    {
        if (!abilityController.pullOff)
        {
            base.Update();
        }
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
