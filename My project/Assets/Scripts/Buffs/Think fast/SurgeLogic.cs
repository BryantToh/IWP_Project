using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SurgeLogic : MonoBehaviour
{
    private Dictionary<NavMeshAgent, (float acceleration, float speed, float attackTiming)> originalValues =
        new Dictionary<NavMeshAgent, (float acceleration, float speed, float attackTiming)>();

    private NavMeshAgent[] agents;
    public GameObject panel;
    public bool attackDodged = false;
    private bool buffActive = false;
    private bool isOnCooldown = false;
    private bool coroutineActive = false;
    public bool isUse = false;

    private const float BuffDurationTime = 5f;
    private const float CooldownTime = 6f;
    private float cooldownTimer;

    private const string EnemyTag = "EnemyObj";

    private void Start()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        if (HandleCooldown())
            return;

        if (attackDodged && !buffActive && !coroutineActive)
        {
            ActivateBuff();
        }

        if (buffActive)
        {
            ApplyBuff();
        }
    }

    private bool HandleCooldown()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        return isOnCooldown;
    }

    private void ActivateBuff()
    {
        buffActive = true;
        panel.SetActive(true);
        StartCoroutine(BuffDuration());
        AudioManager.instance.PlaySFX("dodge");
        isUse = true;
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
                originalValues[agent] = (agent.acceleration, agent.speed, enemyAI.timeBetweenAttacks);

                agent.acceleration /= 5;
                agent.speed /= 5;
                enemyAI.timeBetweenAttacks *= 5;
            }
        }
    }

    private void ResetBuff()
    {
        foreach (var entry in originalValues)
        {
            var agent = entry.Key;
            var (originalAcceleration, originalSpeed, originalAttackTiming) = entry.Value;

            agent.acceleration = originalAcceleration;
            agent.speed = originalSpeed;

            var enemyAI = agent.GetComponent<EnemyAIController>();
            enemyAI.timeBetweenAttacks = originalAttackTiming;
        }

        originalValues.Clear();
        buffActive = false;
        coroutineActive = false;
        isOnCooldown = true;
        cooldownTimer = CooldownTime;
        attackDodged = false;
        panel.SetActive(false);
    }

    private IEnumerator BuffDuration()
    {
        coroutineActive = true;
        yield return new WaitForSeconds(BuffDurationTime);
        ResetBuff();
    }
}
