using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your Enemy prefab
    public Transform spawnPoint; // Assign your spawn location
    public Transform storeWaypoint; // Assign the store waypoint
    
    // Reference to ScoreManage
    private ScoreManage scoreManager;
    private bool spawningEnabled = false; // Control spawning until player earns at least one star

    // Define spawn intervals for each star level
    public float[] spawnIntervalsPerStar = { 10f, 8f, 6f, 4f, 2f }; // Adjust as necessary (e.g., 1 star = 10s, 5 stars = 2s)
    private const int maxStars = 5; // Maximum star limit

    private void Start()
    {
        // Locate the ScoreManage script in the scene
        scoreManager = FindObjectOfType<ScoreManage>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManage script not found in the scene!");
            return;
        }

        // Start checking for player's stars periodically
        StartCoroutine(CheckStarCondition());
    }

    private IEnumerator CheckStarCondition()
    {
        while (!spawningEnabled)
        {
            int stars = scoreManager.StarCount; // Check player's star count
            if (stars >= 1)
            {
                spawningEnabled = true; // Enable spawning once the player earns at least one star
                StartCoroutine(SpawnEnemies()); // Start spawning enemies
            }
            yield return new WaitForSeconds(1f); // Check every second
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawningEnabled) // Continuous spawning
        {
            int stars = Mathf.Clamp(scoreManager.StarCount, 0, maxStars); // Get current stars, clamped between 0 and max

            if (stars >= 1) // Ensure stars are at least 1 (extra safety check)
            {
                float spawnInterval = spawnIntervalsPerStar[stars - 1]; // Use spawn interval based on star count

                SpawnEnemy(); // Spawn the enemy
                yield return new WaitForSeconds(spawnInterval); // Wait for the dynamic interval
            }
            else
            {
                yield return null; // Do nothing if stars drop below 1 (unlikely, but for safety)
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.SetInitialTarget(storeWaypoint); // Direct the AI to the store
        }
    }
}