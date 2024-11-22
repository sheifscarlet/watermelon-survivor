using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnRate = 2f;
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 20f;
    public float forwardExclusionAngle = 30f;
    public int maxActiveEnemies = 100;

    [Header("Terrain and NavMesh Settings")]
    public Terrain terrain;
    public bool checkNavMesh = true;
    public float navMeshCheckRadius = 1f;

    [Header("Spawn Probabilities")]
    [Range(0f, 1f)] public float kamikazeSpawnProbability = 0.2f;

    private float spawnTimer;
    private int currentActiveEnemies;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate && currentActiveEnemies < maxActiveEnemies)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        bool isValidSpawn = false;

        do
        {
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            float angle = Random.Range(0f, 360f);

            Vector3 offset = new Vector3(
                distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                0,
                distance * Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            spawnPosition = player.position + offset;

            if (terrain != null)
            {
                spawnPosition.y = terrain.SampleHeight(spawnPosition) + terrain.transform.position.y;
            }

            isValidSpawn = IsSpawnPositionValid(spawnPosition);

        } while (!isValidSpawn);

        bool spawnKamikaze = Random.value < kamikazeSpawnProbability;

        GameObject enemy = EnemyPool.instance.GetEnemy(spawnKamikaze);

        if (enemy != null)
        {
            if (IsOnNavMesh(spawnPosition))
            {
                enemy.transform.position = spawnPosition;
                enemy.transform.rotation = Quaternion.identity;
                currentActiveEnemies++;
            }
            else
            {
                //Debug.LogWarning("Spawned enemy is not on NavMesh");
                EnemyPool.instance.ReturnEnemy(enemy, spawnKamikaze);
            }
        }
        else
        {
            //Debug.LogWarning("No available enemies in the pool");
        }
    }

    bool IsSpawnPositionValid(Vector3 spawnPosition)
    {
        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;

            if (spawnPosition.x < terrainPosition.x ||
                spawnPosition.z < terrainPosition.z ||
                spawnPosition.x > terrainPosition.x + terrainData.size.x ||
                spawnPosition.z > terrainPosition.z + terrainData.size.z)
            {
                return false;
            }
        }

        Vector3 directionToSpawn = (spawnPosition - player.position).normalized;
        float angleToPlayerForward = Vector3.Angle(player.forward, directionToSpawn);
        return angleToPlayerForward > forwardExclusionAngle / 2;
    }

    private bool IsOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, navMeshCheckRadius, NavMesh.AllAreas);
    }

    public void DecrementActiveEnemies()
    {
        currentActiveEnemies = Mathf.Max(0, currentActiveEnemies - 1);
    }
}
