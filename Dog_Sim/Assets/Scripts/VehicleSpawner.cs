using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager: MonoBehaviour {
    [Header("Spawn Settings")]
    public List<GameObject> vehiclePrefabs; // List of vehicle prefabs
    public Transform spawnPoint;           // Where vehicles spawn
    public Transform despawnPoint;         // Where vehicles despawn
    public float spawnInterval = 2.0f;     // Time between spawns

    [Header("Movement Settings")]
    public float vehicleSpeed = 5f;        // Speed of vehicles

    // Internal list to track active vehicles
    private List<GameObject> activeVehicles = new List<GameObject>();

    void Start() {
        // Start spawning vehicles repeatedly
        StartCoroutine(SpawnVehicles());
    }

    void Update() {
        // Move vehicles towards the despawn point
        for (int i = activeVehicles.Count - 1; i >= 0; i--) {
            GameObject vehicle = activeVehicles[i];

            // Calculate direction from vehicle to despawnPoint
            Vector3 direction = (despawnPoint.position - vehicle.transform.position).normalized;

            // Move vehicle in the calculated direction
            vehicle.transform.Translate(direction * vehicleSpeed * Time.deltaTime, Space.World);

            // Check if vehicle has reached or passed the despawn point
            if (Vector3.Distance(vehicle.transform.position, despawnPoint.position) < 0.5f) {
                // Remove and destroy the vehicle
                activeVehicles.RemoveAt(i);
                Destroy(vehicle);
            }
        }
    }

    IEnumerator SpawnVehicles() {
        while (true) {
            if (vehiclePrefabs.Count > 0) {
                // Select a random prefab from the list
                GameObject prefabToSpawn = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];

                // Spawn the vehicle at the spawn point
                GameObject spawnedVehicle = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

                // Add the spawned vehicle to the active list
                activeVehicles.Add(spawnedVehicle);
            }

            // Wait before the next spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}