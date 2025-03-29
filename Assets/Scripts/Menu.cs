using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button startGame;
    [SerializeField] Button howToPlay;
    [SerializeField] Button exitGame;

    void Start()
    {
        startGame.onClick.AddListener(StartGame);
        howToPlay.onClick.AddListener(HowToPlayGame);
        exitGame.onClick.AddListener(ExitGame);
    }

    void StartGame()
    {
        // GO TO LOADLEVEL SCENE TO SELECT DIFFICULTY
        SceneManager.LoadScene("LoadLevel");
    }

    void HowToPlayGame()
    {
        // SHOW A PANEL HOW TO PLAY NONOGRAMS
    }

    void ExitGame()
    {
        Application.Quit();
    }    
}
