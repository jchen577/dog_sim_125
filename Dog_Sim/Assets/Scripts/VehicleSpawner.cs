using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code modified from ChatGPT

public class VehicleManager: MonoBehaviour {
    [Header("Spawn Settings")]
    public List<GameObject> vehiclePrefabs;
    public Transform spawnPoint;
    public Transform despawnPoint;
    public float spawnInterval = 2.0f;

    [Header("Movement Settings")]
    public float vehicleSpeed = 5f;

    private List<GameObject> activeVehicles = new List<GameObject>();

    void Start() {
        StartCoroutine(SpawnVehicles());
    }

    void Update() {
        // Move vehicles towards despawn point
        for (int i = activeVehicles.Count - 1; i >= 0; i--) {
            GameObject vehicle = activeVehicles[i];

            Vector3 direction = (despawnPoint.position - vehicle.transform.position).normalized;

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
                GameObject prefabToSpawn = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Count)];

                GameObject spawnedVehicle = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

                activeVehicles.Add(spawnedVehicle);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}