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
        // Calculate the total width that the items will occupy.
        float totalWidth = (maxItemsPerRow - 1) * itemSpacing;

        // Calculate the starting position for the row (centered around the rowPoint).
        Vector3 startPoint = rowPoint.position - rowPoint.right * (totalWidth / 2);

        for (int i = 0; i < maxItemsPerRow; i++) {
            // Calculate each item's spawn position along the row.
            Vector3 spawnPosition = startPoint + rowPoint.right * (i * itemSpacing);

            // Randomly select an item prefab (allowing repeats).
            GameObject randomPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // Instantiate the item at the calculated position, parented to the rowPoint.
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