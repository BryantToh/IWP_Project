using UnityEngine;
using UnityEngine.AI;

public class SentinelEnemy : EnemyAIController
{
    SentinelHealth enemyHealth;
    Collider playerCol;
    public ParticleSystem attackNotice;
    public PullAbilityObj pullAbilityObj;
    protected override void Start()
    {
        pullAbilityObj = GameObject.FindGameObjectWithTag("Abilityholder").GetComponent<PullAbilityObj>();
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<SentinelHealth>();
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

            float noticeTime = timeBetweenAttacks - 0.2f;
            if (noticeTime > 0)
                //Invoke(nameof(PlayAttackNotice), noticeTime);

            enemyHealth.AttackPlayer(playerCol);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void PlayAttackNotice()
    {
        if (attackNotice != null)
        {
            attackNotice.Play();
        }
    }

    protected override void ResetAttack()
    {
        base.ResetAttack();
        enemyHealth.AttackReset(playerCol);
    }
}
