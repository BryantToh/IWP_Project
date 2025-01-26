using UnityEngine;
using UnityEngine.AI;

public class PhaseEnemy : EnemyAIController
{
    PhaseHealth enemyHealth;
    Collider playerCol;
    PhaseTransparent phasing;
    public Animator animator;
    public ParticleSystem attackNotice;
    NavMeshAgent navMesh;
    public PullAbilityObj pullAbilityObj;
    protected override void Start()
    {
        pullAbilityObj = GameObject.FindGameObjectWithTag("Abilityholder").GetComponent<PullAbilityObj>();
        player = GameObject.FindGameObjectWithTag("PlayerObj").transform;
        phasing = GetComponent<PhaseTransparent>();
        playerCol = player.gameObject.GetComponent<Collider>();
        enemyHealth = GetComponent<PhaseHealth>();
        navMesh = GetComponent<NavMeshAgent>();
        phasing.OnFadeComplete += OnFadeComplete;
        base.Start();
    }

    protected override void Update()
    {
        if (pullAbilityObj.pullOff)
            return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
            playerInAttackRange = true;
        else
            playerInAttackRange = false;

        if (playerInAttackRange)
        {
            Attacking();
        }
        else
        {
            Phasing();
        }
    }

    private void Phasing()
    {
        animator.SetBool("Moving", true);
        navMesh.speed = 0f;
        phasing.FadeOut();
    }

    private void OnFadeComplete()
    {
        if (phasing.faded)
        {
            Chasing();
        }
    }

    protected override void Chasing()
    {
        animator.SetBool("Moving", true);
        navMesh.speed = 6f;
        base.Chasing();
    }

    protected override void Attacking()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            animator.SetBool("Moving", false);
            phasing.FadeIn();
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

    private void OnDestroy()
    {
        phasing.OnFadeComplete -= OnFadeComplete; // Unsubscribe to avoid memory leaks
    }
}
