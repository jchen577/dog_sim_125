using System.Collections.Generic;
using UnityEngine;

public class ShelfPopulator: MonoBehaviour {
    public Transform[] rackRows; // Array of spawn points representing each row on the shelf.
    public GameObject[] itemPrefabs; // Array of available item prefabs to spawn.
    [Range(1, 100)] public int maxItemsPerRow = 5; // Maximum number of items to place in one row.
    public float itemSpacing = 1.0f; // Distance between items in a single row.

    void Start() {
        PopulateShelf();
    }

    void PopulateShelf() {
        foreach (Transform rackRow in rackRows) {
            SpawnItemsInRow(rackRow);
        }
    }

    void SpawnItemsInRow(Transform rowPoint) {
        // Cap the number of items by the user-defined maximum.
        int itemsToSpawn = Mathf.Min(maxItemsPerRow, itemPrefabs.Length);

        // Randomize item placement by shuffling the prefabs.
        List<GameObject> randomizedItems = new List<GameObject>(itemPrefabs);
        ShuffleList(randomizedItems);

        // Calculate the starting offset for the row (centered around the rowPoint).
        float totalWidth = (itemsToSpawn - 1) * itemSpacing;
        Vector3 startPoint = rowPoint.position - rowPoint.right * (totalWidth / 2);

        for (int i = 0; i < itemsToSpawn; i++) {
            // Calculate spawn position for each item.
            Vector3 spawnPosition = startPoint + rowPoint.right * (i * itemSpacing); // Spread along local X-axis.

            // Randomly pick an item and spawn it.
            GameObject itemPrefab = randomizedItems[Random.Range(0, randomizedItems.Count)];
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity, rowPoint);
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