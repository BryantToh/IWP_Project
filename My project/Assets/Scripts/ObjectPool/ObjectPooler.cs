using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class WaveSystem
    {
        public string waveNumb;
        public List<Pool> pools;
        //public bool waveDone;
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;
    public List<WaveSystem> waves;
    private Dictionary<string, ObjectPool<IPooledEnemy>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, ObjectPool<IPooledEnemy>>();

        foreach (WaveSystem wave in waves)
        {
            foreach (Pool pool in wave.pools)
            {
                if (!poolDictionary.ContainsKey(pool.tag))
                {
                    //Create Function
                    ObjectPool<IPooledEnemy> objectPool = new ObjectPool<IPooledEnemy>(
                        () =>
                        {
                            GameObject obj = Instantiate(pool.prefab);
                            IPooledEnemy r = obj.GetComponent<IPooledEnemy>();
                            r.OnEnemySpawn();
                            obj.gameObject.SetActive(false);
                            return r;
                        },
                        //Get Function
                        (obj) => obj.OnGet(),
                        //Release Function
                        (obj) => obj.OnRelease(),
                        //Destroy Function
                        (obj) => obj.OnDestroyInterface(),
                        true, pool.size
                    );

                    poolDictionary.Add(pool.tag, objectPool);
                }
            }
        }
    }

    public IPooledEnemy SpawnfromPool(string tag)
    {
        if (poolDictionary.TryGetValue(tag, out ObjectPool<IPooledEnemy> pool))
        {
            return pool.Get();
        }
        return null;
    }

    public void Release(string tag, IPooledEnemy obj)
    {
        if (poolDictionary.TryGetValue(tag, out ObjectPool<IPooledEnemy> pool))
        {
            pool.Release(obj);
        }
    }
}
