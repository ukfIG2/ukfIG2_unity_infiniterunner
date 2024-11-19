using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabForRoadWithTrees;
    [SerializeField] private GameObject _prefabForRoadWithCorn;
    [SerializeField] private GameObject _playerCar;

    public enum GameMode { SinglePlayer, OneEnemy, HardCore }
    [SerializeField]  public static GameMode CurrentGameMode;

    private const int TotalRoadsToSpawn = 30;
    private const int DestroyThreshold = 4*20; // How many roads are destroyed before spawning a new batch
    private const float RoadOffsetZ = 30f;   // Distance between consecutive road segments

    private Vector3 _currentSpawnPosition = Vector3.zero; // Tracks where the next road will be spawned
    private bool _spawnTreesNext = true;

    [SerializeField]    private int _destroyedRoadsCount = 0; // Tracks the number of destroyed roads

    public void Awake()
    {
        // Optional: Initialize if needed
        Physics.gravity = new Vector3(0, -20f, 0);
    }

    public void Start()
    {
        // Spawn the initial batch of roads
        SpawnNextRoadBatch();

        // Spawn the player at the first road position
        SpawnPlayer();
        Debug.Log("Starting " + CurrentGameMode);
    }

    public void Update()
    {
        // Logic to trigger spawning based on player's position or other criteria can go here
    }

    public void SelectGameMode(GameMode mode)
    {
        CurrentGameMode = mode;
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

private void SpawnPlayer()
{
    // Position the player at the start of the first road with a slight forward offset
    Vector3 playerStartPosition = new Vector3(0f, 1f, 0f);

    // Spawn the player car at the starting position
    GameObject player = Instantiate(_playerCar, playerStartPosition, Quaternion.identity);

    // Find the Main Camera in the scene
    Camera mainCamera = Camera.main;

    if (mainCamera != null)
    {
        // Set the Main Camera as a child of the player
        mainCamera.transform.SetParent(player.transform);

        // Optionally, adjust the position of the camera relative to the player
        mainCamera.transform.localPosition = new Vector3(0f, 5f, -10f);
        mainCamera.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);
    }
    else
    {
        Debug.LogWarning("Main Camera not found in the scene.");
    }

    // Find the Spot Light in the scene
    Light spotLight = FindObjectOfType<Light>();

    if (spotLight != null && spotLight.type == UnityEngine.LightType.Spot)
    {
        // Set the Spot Light as a child of the player
        spotLight.transform.SetParent(player.transform);

        // Optionally, adjust the position of the light relative to the player
        spotLight.transform.localPosition = new Vector3(0f, 12f, -29f);
        spotLight.transform.localRotation = Quaternion.Euler(9f, 0f, 0f);
    }
    else
    {
        Debug.LogWarning("Spot Light not found in the scene.");
    }
}

}
