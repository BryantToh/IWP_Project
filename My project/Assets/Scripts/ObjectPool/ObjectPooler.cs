using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, ObjectPool<IPooledEnemy>>();
        foreach (Pool pool in pools)
        {
            ObjectPool<IPooledEnemy> objectPool = new ObjectPool<IPooledEnemy>(
                //create function
                () =>
                {
                    GameObject obj = Instantiate(pool.prefab);
                    IPooledEnemy r = obj.GetComponent<IPooledEnemy>();
                    r.OnEnemySpawn();
                    obj.gameObject.SetActive(false);
                    return r;
                },
                //get function
                (obj) =>
                {
                    obj.OnGet();
                },
                //release function
                (obj) =>
                {
                    obj.OnRelease();
                },
                //destroy function
                (obj) =>
                {
                    obj.OnDestroyInterface();
                }, true, pool.size
                );
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, ObjectPool<IPooledEnemy>> poolDictionary;

    public IPooledEnemy SpawnfromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary.TryGetValue(tag, out ObjectPool<IPooledEnemy> pool))
        {
            IPooledEnemy obj = pool.Get();
            obj.SetPosNRot(position, rotation);
            return obj;
        }
        return null;
    }

    public Pool GetPool(string tag)
    {
        return pools.FirstOrDefault(pool => pool.tag == tag);
    }

    public void Release(string tag, IPooledEnemy Obj)
    {
        if (poolDictionary.TryGetValue(tag, out ObjectPool<IPooledEnemy> pool))
        {
            pool.Release(Obj);
        }
    }
}
