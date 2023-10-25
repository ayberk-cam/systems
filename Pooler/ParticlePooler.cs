using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ParticlePooler : MonoBehaviour
{
    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region singleton
    public static ParticlePooler instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }

        CreatePool();
    }
    #endregion

    void RegulateQueue(Queue<GameObject> enemyQueue)
    {
        GameObject[] queueArray = enemyQueue.ToArray(); // keep a reference to iterate over

        foreach (GameObject bullet in queueArray)
        {
            if (bullet.activeSelf)
            {
                GameObject activeEnemy = enemyQueue.Dequeue(); // remove the next active enemy

                enemyQueue.Enqueue(activeEnemy); // add the removed active enemy to end of queue
            }
            else
            {
                break;
            }
        }
    }

    public GameObject SpawnFromPoolParticle(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + " doesn't exist!!");

            return null;
        }

        RegulateQueue(poolDictionary[tag]);

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        if (objectToSpawn.activeSelf)
        {
            poolDictionary[tag].Enqueue(objectToSpawn);
            objectToSpawn = Instantiate(objectToSpawn);
        }

        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = position;

        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private void CreatePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var unit in pools)
        {
            Queue<GameObject> particlePool = new Queue<GameObject>();

            for (int i = 0; i < unit.size; i++)
            {
                unit.prefab.SetActive(false);

                GameObject particle = Instantiate(unit.prefab);

                particle.SetActive(false);

                particlePool.Enqueue(particle);
            }
            poolDictionary.Add(unit.tag, particlePool);
        }
    }
}
