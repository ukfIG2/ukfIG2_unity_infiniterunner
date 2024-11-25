using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabForRoadWithTrees;
    [SerializeField] private GameObject _prefabForRoadWithCorn;
    [SerializeField] private GameObject _playerCar;

    public enum GameMode { SinglePlayer, OneEnemy, HardCore }
    [SerializeField] public static GameMode CurrentGameMode;

    private const int TotalRoadsToSpawn = 30;
    private const int DestroyThreshold = 4 * 20; // How many roads are destroyed before spawning a new batch
    private const float RoadOffsetZ = 30f;   // Distance between consecutive road segments

    private Vector3 _currentSpawnPosition = Vector3.zero; // Tracks where the next road will be spawned
    private bool _spawnTreesNext = true;

    [SerializeField] private int _destroyedRoadsCount = 0; // Tracks the number of destroyed roads

    public static int _level;
    [SerializeField] private int _score;
    [SerializeField] private int _nextScore;

    public static bool _gameOver;

    [SerializeField] private GameObject _pauseMenuUI;  // Reference to the Pause Menu UI
    [SerializeField] private GameObject _gameOverUI;  // Reference to the Game Over UI
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText; // Reference to the score text in the UI
    [SerializeField] private TMPro.TextMeshProUGUI _levelText; // Reference to the level text in the UI
    [SerializeField] private TMPro.TextMeshProUGUI _pauseButtonText; // Reference to Pause/Resume button text

    private bool _isPaused;

    public void Awake()
    {
        Physics.gravity = new Vector3(0, -20f, 0);
        _gameOver = false;
        _isPaused = false;

        if (_pauseMenuUI != null) _pauseMenuUI.SetActive(false);
        if (_gameOverUI != null) _gameOverUI.SetActive(false);
    }

    public void Start()
    {
        Time.timeScale = 1;
        _level = 1;
        _score = 0;
        _nextScore = 2000;

        SpawnNextRoadBatch();
        SpawnPlayer();
        

        Debug.Log("Starting " + CurrentGameMode);
    }

    public void Update()
    {
        if (_gameOver)
        {
            HandleGameOver();
            return;
        }

        HandlePause();

        // Update score and level progression
        _score += (int)(Time.deltaTime * 100); // Score increments over time
        if (_score >= _nextScore)
        {
            _level++;
            _nextScore += 2000;
            UpdateUI(); // Update UI when level changes
        }

        UpdateUI();
    }

    private void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
    }

    // TogglePause method for use in UI buttons
    public void TogglePause()
    {
        if (_isPaused)
        {
            ResumeGame();
            if (_pauseButtonText != null) _pauseButtonText.text = "Pause";
        }
        else
        {
            PauseGame();
            if (_pauseButtonText != null) _pauseButtonText.text = "Resume";
        }
    }

    private void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
        if (_pauseMenuUI != null) _pauseMenuUI.SetActive(true);
    }

    private void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1;
        if (_pauseMenuUI != null) _pauseMenuUI.SetActive(false);
    }

    private void HandleGameOver()
    {
        Time.timeScale = 0;
        if (_gameOverUI != null) _gameOverUI.SetActive(true);

        if (_scoreText != null) _scoreText.text = $"Final Score: {_score}";
        if (_levelText != null) _levelText.text = $"Level Reached: {_level}";
    }

    public void NotifyRoadDestroyed()
    {
        _destroyedRoadsCount++;
        if (_destroyedRoadsCount >= DestroyThreshold)
        {
            _destroyedRoadsCount = 0;
            SpawnNextRoadBatch();
        }
    }

    private void SpawnNextRoadBatch()
    {
        for (int i = 0; i < TotalRoadsToSpawn; i++)
        {
            SpawnRoadSegment();
        }

        _spawnTreesNext = !_spawnTreesNext;
    }

    private void SpawnRoadSegment()
    {
        GameObject newRoadSegment = _spawnTreesNext ? Instantiate(_prefabForRoadWithTrees) : Instantiate(_prefabForRoadWithCorn);
        newRoadSegment.transform.position = _currentSpawnPosition;
        _currentSpawnPosition.z += RoadOffsetZ;
    }

    private void SpawnPlayer()
    {
        Vector3 playerStartPosition = new Vector3(0f, 1f, 0f);
        GameObject player = Instantiate(_playerCar, playerStartPosition, Quaternion.identity);

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.SetParent(player.transform);
            mainCamera.transform.localPosition = new Vector3(0f, 5f, -10f);
            mainCamera.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);
        }

        Light spotLight = FindObjectOfType<Light>();
        if (spotLight != null && spotLight.type == LightType.Spot)
        {
            spotLight.transform.SetParent(player.transform);
            spotLight.transform.localPosition = new Vector3(0f, 12f, -29f);
            spotLight.transform.localRotation = Quaternion.Euler(9f, 0f, 0f);
        }
    }

    private void UpdateUI()
    {
        if (_scoreText != null) _scoreText.text = $"Score: {_score}";
        if (_levelText != null) _levelText.text = $"Level: {_level}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        _gameOver = true;
    }
}

