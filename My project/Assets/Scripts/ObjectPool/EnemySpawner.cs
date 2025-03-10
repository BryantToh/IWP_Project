using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler pooler;
    private int currentWaveIndex = 0;
    private int activeEnemies = 0;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex >= pooler.waves.Count)
            return;

        var currentWave = pooler.waves[currentWaveIndex];
        activeEnemies = 0;

        foreach (var pool in currentWave.pools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                SpawnEnemy(pool.tag);
            }
        }
    }

    private void SpawnEnemy(string tag)
    {
        IPooledEnemy enemy = pooler.SpawnfromPool(tag);
        if (enemy != null)
        {
            enemy.SetPosNRot(randomPos(), Quaternion.identity);
            activeEnemies++;
        }
    }

    public void OnEnemyDefeated(/*IPooledEnemy enemy, string tag*/)
    {
        //pooler.Release(tag, enemy);
        activeEnemies--;

        if (activeEnemies <= 0)
        {
            currentWaveIndex++;
            StartNextWave();
        }
    }

    private Vector3 randomPos()
    {
        return new Vector3(
            Random.Range(-10, 10),
            0,
            Random.Range(-10, 10)
        );
    }
}
