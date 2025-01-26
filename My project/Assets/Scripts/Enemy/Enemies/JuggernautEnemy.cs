using UnityEngine;

public class JuggernautEnemy : EnemyAIController
{
    Collider playerCol;
    JuggernautHealth enemyHealth;
    public PullAbilityObj pullAbilityObj;
    public Animator animator;
    public ParticleSystem attackNotice;
    protected override void Start()
    {
        pullAbilityObj = GameObject.FindGameObjectWithTag("Abilityholder").GetComponent<PullAbilityObj>();
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<JuggernautHealth>();
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
