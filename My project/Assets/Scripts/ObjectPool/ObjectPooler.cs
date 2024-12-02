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

            //for (int i = 0; i < pool.size; i++)
            //{
            //    GameObject obj = Instantiate(pool.prefab);
            //    obj.SetActive(false);
            //    objectPool.Enqueue(obj);
            //}

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
        //if (!poolDictionary.ContainsKey(tag)) 
        //    return null;

        //if (poolDictionary[tag].Count > 0)
        //{
        //    GameObject r = poolDictionary[tag].Dequeue();
        //    r.SetActive(true);
        //    r.transform.position = position;
        //    r.transform.rotation = rotation;
        //}

        //GameObject objToSpawn = poolDictionary[tag].Dequeue();
        //objToSpawn.SetActive(true);
        //objToSpawn.transform.position = position;
        //objToSpawn.transform.rotation = rotation;

        //IPooledEnemy pooledEnemy = objToSpawn.GetComponent<IPooledEnemy>();

        //if (pooledEnemy != null)
        //{
        //    pooledEnemy.OnEnemySpawn();
        //}

        //return objToSpawn;
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
