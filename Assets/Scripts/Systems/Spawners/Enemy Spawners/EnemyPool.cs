using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    [Header("Standard Enemy Pool")]
    public GameObject standardEnemyPrefab;
    public int standardPoolSize = 10;

    [Header("Kamikaze Enemy Pool")]
    public GameObject kamikazeEnemyPrefab;
    public int kamikazePoolSize = 5;

    public Terrain terrain;
    public float navMeshCheckRadius = 1f;

    private Queue<GameObject> standardEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> kamikazeEnemyPool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < standardPoolSize; i++)
        {
            Vector3 validPosition = GetValidSpawnPosition();
            GameObject enemy = Instantiate(standardEnemyPrefab, validPosition, Quaternion.identity);
            enemy.SetActive(false);
            standardEnemyPool.Enqueue(enemy);
        }

        for (int i = 0; i < kamikazePoolSize; i++)
        {
            Vector3 validPosition = GetValidSpawnPosition();
            GameObject kamikaze = Instantiate(kamikazeEnemyPrefab, validPosition, Quaternion.identity);
            kamikaze.SetActive(false);
            kamikazeEnemyPool.Enqueue(kamikaze);
        }
    }

    public GameObject GetEnemy(bool isKamikaze)
    {
        if (isKamikaze)
        {
            if (kamikazeEnemyPool.Count > 0)
            {
                GameObject enemy = kamikazeEnemyPool.Dequeue();
                enemy.SetActive(true);
                return enemy;
            }
        }
        else
        {
            if (standardEnemyPool.Count > 0)
            {
                GameObject enemy = standardEnemyPool.Dequeue();
                enemy.SetActive(true);
                return enemy;
            }
        }
        //Debug.LogWarning("No available enemies in the pool");
        return null;
    }

    public void ReturnEnemy(GameObject enemy, bool isKamikaze)
    {
        enemy.SetActive(false);
        if (isKamikaze)
        {
            kamikazeEnemyPool.Enqueue(enemy);
        }
        else
        {
            standardEnemyPool.Enqueue(enemy);
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        bool isValid = false;

        do
        {
            if (terrain != null)
            {
                float x = Random.Range(terrain.transform.position.x, terrain.transform.position.x + terrain.terrainData.size.x);
                float z = Random.Range(terrain.transform.position.z, terrain.transform.position.z + terrain.terrainData.size.z);
                float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;

                randomPosition = new Vector3(x, y, z);
            }

            isValid = IsOnNavMesh(randomPosition);

        } while (!isValid);

        return randomPosition;
    }

    private bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, navMeshCheckRadius, NavMesh.AllAreas);
    }
}
