using UnityEngine;

public class MindBreakersEnemy : EnemyAIController
{
    Collider playerCol;
    MindbreakersHealth enemyHealth;
    public PullAbilityObj pullAbilityObj;
    public Animator animator;
    public ParticleSystem attackNotice;
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
        animator.SetBool("Moving", true);
        base.Chasing();
    }

    protected override void Attacking()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            animator.SetBool("Moving", false);
            alreadyAttacked = true;
            float noticeTime = timeBetweenAttacks - 0.2f;
            if (noticeTime > 0)
                Invoke(nameof(PlayAttackNotice), noticeTime);
            enemyHealth.AttackPlayer(playerCol);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void PlayAttackNotice()
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
