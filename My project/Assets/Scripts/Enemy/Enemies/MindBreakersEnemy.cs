using UnityEngine;

public class MindBreakersEnemy : EnemyAIController
{
    Collider playerCol;
    MindbreakersHealth enemyHealth;
    public PullAbilityObj pullAbilityObj;
    protected override void Start()
    {
        pullAbilityObj = GameObject.FindGameObjectWithTag("Abilityholder").GetComponent<PullAbilityObj>();
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<MindbreakersHealth>();
        base.Start();
    }

    protected override void Update()
    {
        //if (!pullAbilityObj.pullOff)
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
