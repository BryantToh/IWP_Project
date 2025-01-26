using UnityEngine;

public class OverseerEnemy : EnemyAIController
{
    Collider playerCol;
    OverseerHealth enemyHealth;
    public float rangedAttack;
    public float meleeAttack;
    public Animator animator;
    public ParticleSystem attackNotice;
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
        animator.SetBool("Moving", true);
        base.Chasing();
    }

    protected override void Attacking()
    {
        agent.SetDestination(transform.position);

        if (Vector3.Distance(transform.position, player.position) > attackRange)
            return;

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
