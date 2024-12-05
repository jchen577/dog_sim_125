using System.Collections.Generic;
using UnityEngine;

// Code modified from ChatGPT

public class ShelfPopulator: MonoBehaviour {
    public Transform[] spawnPoints;
    public GameObject[] itemPrefabs;
    [Range(1, 100)] public int maxItemsPerRow = 5;
    public float itemSpacing = 1.0f;

    void Start() {
        PopulateShelf();
    }

    void PopulateShelf() {
        foreach (Transform rackRow in spawnPoints) {
            SpawnItemsInRow(rackRow);
        }
    }

    void SpawnItemsInRow(Transform rowPoint) {
        float totalWidth = (maxItemsPerRow - 1) * itemSpacing;

        Vector3 startPoint = rowPoint.position - rowPoint.right * (totalWidth / 2);

        for (int i = 0; i < maxItemsPerRow; i++) {
            Vector3 spawnPosition = startPoint + rowPoint.right * (i * itemSpacing);

            GameObject randomPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            Instantiate(randomPrefab, spawnPosition, Quaternion.identity, rowPoint);
        }
    }

    // Utility function to shuffle any list.
    private void ShuffleList<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int randomIndex = Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}