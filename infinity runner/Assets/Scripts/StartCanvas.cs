using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu; // Reference to the Main Menu UI
    [SerializeField] private GameObject _levelMenu; // Reference to the Level Selection UI

    private void Start()
    {
        // Ensure the correct menu is displayed on start
        _levelMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    // Called when the "Play" button is clicked
    public void PlayGame()
    {
        // Activate the Level Menu and deactivate the Main Menu
        _levelMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    // Called when the "Back" button is clicked in the Level Selection Menu
    public void BackToMainMenu()
    {
        // Activate the Main Menu and deactivate the Level Menu
        _mainMenu.SetActive(true);
        _levelMenu.SetActive(false);
    }

    // Called when Level 1 is selected
    public void Level1()
    {
        // Set the game mode to SinglePlayer
        GameManager.CurrentGameMode = GameManager.GameMode.SinglePlayer;

        // Load the main scene
        SceneManager.LoadScene("Main");
    }

    // Called when Level 2 is selected
    public void Level2()
    {
        // Set the game mode to OneEnemy
        GameManager.CurrentGameMode = GameManager.GameMode.OneEnemy;

        // Load the main scene
        SceneManager.LoadScene("Main");
    }

    // Called when Level 3 is selected
    public void Level3()
    {
        // Set the game mode to HardCore
        GameManager.CurrentGameMode = GameManager.GameMode.HardCore;

        // Load the main scene
        SceneManager.LoadScene("Main");
    }

    // Called when the "Quit" button is clicked
    public void QuitGame()
    {
        Application.Quit();
    }
}
