using UnityEngine;
using UnityEngine.AI;

public class PhaseEnemy : EnemyAIController
{
    PhaseHealth enemyHealth;
    Collider playerCol;
    PhaseTransparent phasing;
    NavMeshAgent navMesh;
    protected override void Start()
    {
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
        navMesh.speed = 6f;
        base.Chasing();
    }

    protected override void Attacking()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            phasing.FadeIn();
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

    private void OnDestroy()
    {
        phasing.OnFadeComplete -= OnFadeComplete; // Unsubscribe to avoid memory leaks
    }
}
