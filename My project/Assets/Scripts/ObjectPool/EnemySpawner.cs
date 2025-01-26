using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler pooler;
    public Vector3 minRange;
    public Vector3 maxRange;

    // Enemy pools
    ObjectPooler.Pool sentinelPool, juggernautPool, phasePool, mindPool, sentineltutPool;

    [HideInInspector] public int sentinelOnField = 0;
    [HideInInspector] public int juggernautOnField = 0;
    [HideInInspector] public int phaseOnField = 0;
    [HideInInspector] public int mindOnField = 0;
    [HideInInspector] public int sentineltutOnField = 0;

    // Timers and spawn intervals
    private float sentinelTimer = 0f;
    public float sentinelSpawnInterval;

    private float juggernautTimer = 0f;
    public float juggernautSpawnInterval;

    private float phaseTimer = 0f;
    public float phaseSpawnInterval;

    private float mindTimer = 0f;
    public float mindSpawnInterval;

    // Number of enemies to spawn at the start
    int initialSentinels = 4;
    int initialJuggernauts = 2;
    int initialPhases = 3;
    int initialMinds = 1;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
        sentinelPool = pooler.GetPool("sentinel");
        juggernautPool = pooler.GetPool("juggernaut");
        phasePool = pooler.GetPool("phase");
        mindPool = pooler.GetPool("breaker");
        sentineltutPool = pooler.GetPool("sentineltut");

        SpawnInitialEnemies(); // Spawn initial enemies
    }

    private void Update()
    {
        SpawnEnemies();
    }

    private void SpawnInitialEnemies()
    {
        // Spawn initial sentinels
        for (int i = 0; i < initialSentinels; i++)
        {
            if (sentinelOnField < sentinelPool.size)
            {
                pooler.SpawnfromPool("sentinel", randomPos(), Quaternion.identity);
                sentinelOnField++;
            }
        }

        // Spawn initial juggernauts
        for (int i = 0; i < initialJuggernauts; i++)
        {
            if (juggernautOnField < juggernautPool.size)
            {
                pooler.SpawnfromPool("juggernaut", randomPos(), Quaternion.identity);
                juggernautOnField++;
            }
        }

        // Spawn initial phases
        for (int i = 0; i < initialPhases; i++)
        {
            if (phaseOnField < phasePool.size)
            {
                pooler.SpawnfromPool("phase", randomPos(), Quaternion.identity);
                phaseOnField++;
            }
        }

        // Spawn initial minds
        for (int i = 0; i < initialMinds; i++)
        {
            if (mindOnField < mindPool.size)
            {
                pooler.SpawnfromPool("breaker", randomPos(), Quaternion.identity);
                mindOnField++;
            }
        }
    }

    private void SpawnEnemies()
    {
        float deltaTime = Time.deltaTime;

        // Sentinel
        sentinelTimer += deltaTime;
        if (sentinelPool != null && sentinelOnField < sentinelPool.size && sentinelTimer >= sentinelSpawnInterval)
        {
            pooler.SpawnfromPool("sentinel", randomPos(), Quaternion.identity);
            sentinelOnField++;
            sentinelTimer = 0f; // Reset timer
        }

        // Juggernaut
        juggernautTimer += deltaTime;
        if (juggernautPool != null && juggernautOnField < juggernautPool.size && juggernautTimer >= juggernautSpawnInterval)
        {
            pooler.SpawnfromPool("juggernaut", randomPos(), Quaternion.identity);
            juggernautOnField++;
            juggernautTimer = 0f; // Reset timer
        }

        // Phase
        phaseTimer += deltaTime;
        if (phasePool != null && phaseOnField < phasePool.size && phaseTimer >= phaseSpawnInterval)
        {
            pooler.SpawnfromPool("phase", randomPos(), Quaternion.identity);
            phaseOnField++;
            phaseTimer = 0f; // Reset timer
        }

        // Mind
        mindTimer += deltaTime;
        if (mindPool != null && mindOnField < mindPool.size && mindTimer >= mindSpawnInterval)
        {
            pooler.SpawnfromPool("breaker", randomPos(), Quaternion.identity);
            mindOnField++;
            mindTimer = 0f; // Reset timer
        }

        if (sentineltutPool != null && sentineltutOnField < sentineltutPool.size)
        {
            pooler.SpawnfromPool("sentineltut", randomPos(), Quaternion.identity);
            sentineltutOnField++;
        }
    }

    Vector3 randomPos()
    {
        float randomX = Random.Range(minRange.x, maxRange.x);
        float randomY = Random.Range(minRange.y, maxRange.y);
        float randomZ = Random.Range(minRange.z, maxRange.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
