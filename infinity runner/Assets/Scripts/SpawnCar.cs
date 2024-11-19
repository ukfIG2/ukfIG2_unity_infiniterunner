using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs; // Array of car prefabs
    [SerializeField] private Transform[] carSpawners; // Array of car spawners

    // Start is called before the first frame update
    void Start()
    {
        // Check if there are any car prefabs and spawners assigned
        if (carPrefabs.Length > 0 && carSpawners.Length > 0)
        {
            // Pick a random spawner
            Transform randomSpawner = carSpawners[Random.Range(0, carSpawners.Length)];

            // Pick a random car prefab
            GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

            // Instantiate the random car at the random spawner's position
            Instantiate(randomCarPrefab, randomSpawner.position, randomSpawner.rotation);
        }
        else
        {
            Debug.LogWarning("Car prefabs or spawners not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // No update logic needed for now
    }
}