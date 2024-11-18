using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabForRoadWithTrees;
    [SerializeField] private GameObject _prefabForRoadWithCorn;
    [SerializeField] private GameObject _playerCar;

    public enum GameMode { SinglePlayer, OneEnemy, HardCore }
    private GameMode _currentGameMode;

    private const int TotalRoadsToSpawn = 30;
    private const int DestroyThreshold = 15; // How many roads are destroyed before spawning a new batch
    private const float RoadOffsetZ = 30f;   // Distance between consecutive road segments

    private Vector3 _currentSpawnPosition = Vector3.zero; // Tracks where the next road will be spawned
    private bool _spawnTreesNext = true;

    private int _destroyedRoadsCount = 0; // Tracks the number of destroyed roads

    private Transform _playerTransform;

    public void Awake()
    {
        _playerTransform = _playerCar.transform;
    }

    public void Start()
    {
        // Spawn the initial batch of roads
        SpawnNextRoadBatch();
    }

    public void Update()
    {
        // Logic to trigger spawning based on player's position or other criteria can go here
    }

    public void SelectGameMode(GameMode mode)
    {
        _currentGameMode = mode;
    }

    public void NotifyRoadDestroyed()
    {
        // Increment the destroyed road count
        _destroyedRoadsCount++;

        // Check if a new batch of roads needs to be spawned
        if (_destroyedRoadsCount >= DestroyThreshold)
        {
            _destroyedRoadsCount = 0; // Reset the counter
            SpawnNextRoadBatch();
        }
    }

    private void SpawnNextRoadBatch()
    {
        // Spawn a batch of roads based on the current theme
        for (int i = 0; i < TotalRoadsToSpawn; i++)
        {
            SpawnRoadSegment();
        }

        // Alternate between road themes
        _spawnTreesNext = !_spawnTreesNext;
    }

    private void SpawnRoadSegment()
    {
        GameObject newRoadSegment;

        // Choose the correct prefab based on the theme
        if (_spawnTreesNext)
        {
            newRoadSegment = Instantiate(_prefabForRoadWithTrees);
        }
        else
        {
            newRoadSegment = Instantiate(_prefabForRoadWithCorn);
        }

        // Set the position of the new road segment
        newRoadSegment.transform.position = _currentSpawnPosition;

        // Update the spawn position for the next road segment
        _currentSpawnPosition.z += RoadOffsetZ;
    }
}
