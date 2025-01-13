using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurgeLogic : MonoBehaviour
{
    private Dictionary<NavMeshAgent, (float acceleration, float speed, float attackRange)> originalValues =
        new Dictionary<NavMeshAgent, (float acceleration, float speed, float attackRange)>();

    private NavMeshAgent[] agents;
    public bool attackDodged = false;

    private bool buffActive = false;
    private bool isOnCooldown = false;
    private bool coroutineActive = false;

    private const float BuffDurationTime = 1.5f;
    private const float CooldownTime = 4f;
    private float cooldownTimer;

    private const string EnemyTag = "EnemyObj";

    private void Update()
    {
        HandleCooldown();

        if (attackDodged && !buffActive && !coroutineActive)
        {
            ActivateBuff();
        }

        if (buffActive)
        {
            ApplyBuff();
        }
    }

    private void HandleCooldown()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    private void ActivateBuff()
    {
        buffActive = true;
        StartCoroutine(BuffDuration());
    }

    private void GetEnemies()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(EnemyTag);
        agents = new NavMeshAgent[enemyObjects.Length];

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            agents[i] = enemyObjects[i].GetComponent<NavMeshAgent>();
        }
    }

    private void ApplyBuff()
    {
        if (originalValues.Count > 0) return;

        GetEnemies();
        foreach (NavMeshAgent agent in agents)
        {
            if (!originalValues.ContainsKey(agent))
            {
                var enemyAI = agent.GetComponent<EnemyAIController>();
                originalValues[agent] = (agent.acceleration, agent.speed, enemyAI.attackRange);

                agent.acceleration /= 2;
                agent.speed /= 2;
                enemyAI.attackRange /= 2;
            }
        }
    }

    private void ResetBuff()
    {
        foreach (var entry in originalValues)
        {
            var agent = entry.Key;
            var (originalAcceleration, originalSpeed, originalAttackRange) = entry.Value;

            agent.acceleration = originalAcceleration;
            agent.speed = originalSpeed;

            var enemyAI = agent.GetComponent<EnemyAIController>();
            enemyAI.attackRange = originalAttackRange;
        }

        originalValues.Clear();
        buffActive = false;
        coroutineActive = false;
        isOnCooldown = true;
        cooldownTimer = CooldownTime;
        attackDodged = false;
    }

    private IEnumerator BuffDuration()
    {
        coroutineActive = true;
        yield return new WaitForSeconds(BuffDurationTime);
        ResetBuff();
    }
}
