using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string pool_id;
    public GameObject prefab;
    public int size;

    public Pool(string pool_id, GameObject prefab, int size)
    {
        this.pool_id = pool_id;
        this.prefab = prefab;
        this.size = size;
    }
}



public class PoolManager : SingletonDestroy<PoolManager>
{
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDict;
    void Awake()
    {
        base.Awake();
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDict.Add(pool.pool_id, objectQueue);
        }
    }

    public GameObject GetFromPool(string tag, Vector3 position, Quaternion rotation , bool setpos = true)
    {

        if (poolDict.TryGetValue(tag, out var pool))
        {

            GameObject obj;
            if (pool.Count > 0)
            {
                obj = pool.Dequeue();

                obj.SetActive(true);
            }
            else
            {

                var foundPool = pools.Find(x => x.pool_id == tag);
                if (foundPool == null)
                {
                    Debug.LogWarning("Prefab with tag " + tag + " not found in pool list.");
                    return null;
                }
                obj = Instantiate(foundPool.prefab);
                obj.SetActive(true);

            }

            if (setpos)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
            }


            return obj;
        }
        else
        {

            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }


    }
    public void AddToPool(GameObject obj, string pool_id)
    {
        obj.SetActive(false);
        if (poolDict.TryGetValue(pool_id, out var pool))
        {
            pool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Trying to return to unknown pool: " + pool_id);
        }
    } 
}
