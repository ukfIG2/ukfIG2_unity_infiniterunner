using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    //enter in inspector input text
    // Start is called before the first frame update
    [SerializeField] private TMP_Text maxScore;
    void Start()
    {
        maxScore.text = "Highscore: " + PlayerPrefs.GetInt("PlayerMaxScore");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
