using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    ObjectPooler pooler;
    public Vector3 minRange;
    public Vector3 maxRange;
    ObjectPooler.Pool sentinelPool, juggernautPool;
    public int sentinelOnField = 0;
    public int juggernautOnField = 0;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
        sentinelPool = pooler.GetPool("sentinel");
        juggernautPool = pooler.GetPool("juggernaut");
    }

    private void Update()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (sentinelPool != null && sentinelOnField < sentinelPool.size)
        {
            pooler.SpawnfromPool("sentinel", randomPos(), Quaternion.identity);
            sentinelOnField++;
        }
        if (juggernautPool != null && juggernautOnField < juggernautPool.size)
        {
            pooler.SpawnfromPool("juggernaut", randomPos(), Quaternion.identity);
            juggernautOnField++;
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
