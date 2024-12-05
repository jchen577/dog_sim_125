using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your Enemy prefab here
    public Transform spawnPoint; // Assign a spawn location
    public Transform storeWaypoint; // Assign the store waypoint
    public float spawnInterval = 5f; // Time between spawns

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true) // Continuous spawning
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
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
