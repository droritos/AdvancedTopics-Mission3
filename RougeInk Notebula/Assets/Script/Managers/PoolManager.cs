using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private Transform poolParent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameObject poolObj = GameObject.Find("===== Object Pool =====");
        if (poolObj == null)
        {
            poolObj = new GameObject("===== Object Pool =====");
        }
        poolParent = poolObj.transform;
    }

    public GameObject SpawnFromPool(string tag, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag] = new Queue<GameObject>();
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
            if (parent != null)
                objectToSpawn.transform.SetParent(parent);
            else
                objectToSpawn.transform.SetParent(poolParent);
        }
        else
        {
            if (parent != null)
                objectToSpawn = Instantiate(prefab, position, rotation, parent);
            else
                objectToSpawn = Instantiate(prefab, position, rotation, poolParent);
            
            objectToSpawn.name = tag;
        }

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag] = new Queue<GameObject>();
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}
