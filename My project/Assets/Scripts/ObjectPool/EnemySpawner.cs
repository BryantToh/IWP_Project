using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler pooler;
    public Vector3 minRange;
    public Vector3 maxRange;
    ObjectPooler.Pool sentinelPool;
    int enemiesSpawned = 0;

    private void Start()
    {
        pooler = ObjectPooler.Instance;
        sentinelPool = pooler.GetPool("sentinel");
    }

    private void FixedUpdate()
    {
        if (sentinelPool != null)
        {
            if (enemiesSpawned != 2)
            {
                pooler.SpawnfromPool("sentinel", randomPos(), Quaternion.identity);
                enemiesSpawned++;
            }
        }
    }

    Vector3 randomPos()
    {
        float randomX = Random.Range(minRange.x, maxRange.x);
        float randomY = Random.Range(minRange.y, maxRange.y);
        float randomZ = Random.Range(minRange.z, maxRange.z);

        return new Vector3 (randomX, randomY, randomZ);
    }
}
