using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class SurgeLogic : MonoBehaviour
{
    private Dictionary<NavMeshAgent, (float acceleration, float speed, float attackRange)> originalValues =
    new Dictionary<NavMeshAgent, (float acceleration, float speed, float attackRange)>();

    NavMeshAgent[] agent;
    public bool attackDodged = false;
    bool buffActive = false;
    bool isOnCooldown = false;
    bool buffAlrActive = false;
    bool coroutineActive = false;
    float cooldownTime = 4f;
    float cooldownTimer;
    string enemyTag = "EnemyObj";

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (attackDodged)
        {
            buffActive = true;
            StartCoroutine(BuffDuration());
        }

        if (buffActive)
        {
            buffEffect();
        }
    }
    private void GetEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        agent = new NavMeshAgent[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            agent[i] = enemies[i].GetComponent<NavMeshAgent>();
        }
    }
    private void buffEffect()
    {
        if (buffAlrActive)
            return;

        GetEnemies();
        foreach (NavMeshAgent EnemyAgent in agent)
        {
            if (!originalValues.ContainsKey(EnemyAgent))
            {
                EnemyAIController enemyAIController = EnemyAgent.GetComponent<EnemyAIController>();
                originalValues[EnemyAgent] = (EnemyAgent.acceleration, EnemyAgent.speed, enemyAIController.attackRange);

                EnemyAgent.acceleration /= 2;
                EnemyAgent.speed /= 2;
                enemyAIController.attackRange /= 2;
            }
        }
        buffAlrActive = true;
    }

    private void ResetChanges()
    {
        foreach (var entry in originalValues)
        {
            NavMeshAgent EnemyAgent = entry.Key;
            (float originalAcceleration, float originalSpeed, float originalAttackRange) = entry.Value;

            EnemyAgent.acceleration = originalAcceleration;
            EnemyAgent.speed = originalSpeed;
            EnemyAIController enemyAIController = EnemyAgent.GetComponent<EnemyAIController>();
            enemyAIController.attackRange = originalAttackRange;
        }

        originalValues.Clear();
        cooldownTimer = cooldownTime;
        buffActive = false;
        isOnCooldown = true;
        attackDodged = false;
        buffAlrActive = false;
        coroutineActive = false;
    }

    private IEnumerator BuffDuration()
    {
        if (coroutineActive)
            yield break;
        else
        {
            yield return new WaitForSeconds(1.5f);
            coroutineActive = true;
            ResetChanges();
        }
    }
}
