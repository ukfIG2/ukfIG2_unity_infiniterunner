using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs; // Array of car prefabs
    [SerializeField] private Transform[] carSpawners; // Array of car spawners

    private void Start()
    {
        // Ensure car prefabs and spawners are assigned
        if (carPrefabs.Length == 0 || carSpawners.Length == 0)
        {
            Debug.LogWarning("Car prefabs or spawners are not assigned in the inspector.");
            return;
        }

        // Determine the number of cars to spawn based on the GameManager's level
        int level = Mathf.Clamp(GameManager._level, 1, carSpawners.Length);

        // Create a temporary list of spawners to avoid duplicates
        List<Transform> availableSpawners = new List<Transform>(carSpawners);

        // Spawn the cars
        for (int i = 0; i < level; i++)
        {
            // Select a random spawner from the available spawners
            int spawnerIndex = Random.Range(0, availableSpawners.Count);
            Transform selectedSpawner = availableSpawners[spawnerIndex];

            // Remove the used spawner from the list to avoid duplicates
            availableSpawners.RemoveAt(spawnerIndex);

            // Select a random car prefab
            GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

            // Instantiate the car at the selected spawner's position and rotation
            Instantiate(randomCarPrefab, selectedSpawner.position, selectedSpawner.rotation);

            // If there are no more available spawners, break out of the loop
            if (availableSpawners.Count == 0) break;
        }
    }

    private void Update()
    {
        // No update logic needed for now
    }
}
