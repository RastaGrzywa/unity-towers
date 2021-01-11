using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public GameObject parent;
    }

    public UIController uIController;

    public Pool shotsPool;
    public Pool towersPool;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPooler I;

    public bool towersMaximumAmountReached = false;

    private List<GameObject> _towerObjects;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        _towerObjects = new List<GameObject>();
        CreatePool(shotsPool, null);
        CreatePool(towersPool, _towerObjects);
    }

    private void CreatePool(Pool pool, List<GameObject> createdObjectsList)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();
        CreateObjectsInPool(pool.size, pool.prefab, pool.parent.transform, objectPool, createdObjectsList);
        poolDictionary.Add(pool.tag, objectPool);
    }

    private void CreateObjectsInPool(int amount, GameObject prefab, Transform parent, Queue<GameObject> objectPool,
        List<GameObject> createdObjectsList)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.parent = parent;
            objectPool.Enqueue(obj);
            if (createdObjectsList != null)
            {
                createdObjectsList.Add(obj);
            }
        }
    }

    public GameObject SpawnShotFromPool(Vector3 position, Quaternion rotation)
    {
        string poolTag = "shot";
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning("Pool with tag: " + poolTag + " doesn't exists.");
            return null;
        }

        if (poolDictionary[poolTag].Count == 0)
        {
            CreateObjectsInPool(1, shotsPool.prefab, shotsPool.parent.transform, poolDictionary[poolTag], null);
        }

        return SpawnFromPool(poolTag, position, rotation);
    }

    public GameObject SpawnTowerFromPool(Vector3 position, Quaternion rotation)
    {
        string poolTag = "tower";
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning("Pool with tag: " + poolTag + " doesn't exists.");
            return null;
        }

        if (poolDictionary[poolTag].Count == 0)
        {
            towersMaximumAmountReached = true;
            Debug.LogWarning("Pool with tag: " + poolTag + " doesn't have anymore objects.");
            ActivateTowersLastSequence();
            return null;
        }

        return SpawnFromPool(poolTag, position, rotation);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        IPooledObject pooledObject = objToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        return objToSpawn;
    }

    private void ActivateTowersLastSequence()
    {
        foreach (var towerObject in _towerObjects)
        {
            towerObject.GetComponent<Tower>().ActivateTower();
        }
    }

    public void ReturnObjectToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}