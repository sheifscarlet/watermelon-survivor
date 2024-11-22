using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    [Header("References")]
    public EnemySpawner enemySpawner; 

    [Header("Difficulty Settings")]
    public float initialSpawnRate = 2f; 
    public float minSpawnRate = 0.5f; 
    public float rampDuration = 1800f; // Time to reach the minimum spawn rate

    private TimerController timerController;

    private void Start()
    {
        
        timerController = TimerController.instance;

        if (timerController == null)
        {
            return;
        }

        
        if (enemySpawner != null)
        {
            enemySpawner.spawnRate = initialSpawnRate;
        }
    }

    private void Update()
    {
        if (timerController != null && enemySpawner != null)
        {
            // Get elapsed time
            float elapsedTime = timerController.elapsedTime;

            // Scale the spawn rate based on elapsed time
            float t = Mathf.Clamp01(elapsedTime / rampDuration); 
            enemySpawner.spawnRate = Mathf.Lerp(initialSpawnRate, minSpawnRate, t);
        }
    }
}
