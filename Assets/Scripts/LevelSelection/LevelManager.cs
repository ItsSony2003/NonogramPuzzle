using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Difficulties")]
    [SerializeField] SO_Difficulty[] allDifficulties;
    [SerializeField] Transform difficultyHolder; // BUTTON FOR DIFFICULTIES
    [SerializeField] GameObject difficultyPrefab;

    [Header("Levels")]
    [SerializeField] Transform levelHolder; // BUTTONS FOR LEVEL
    [SerializeField] GameObject levelButtonPrefab;

    [Header("Panels")]
    [SerializeField] GameObject difficultyPanel; // Image/panel for difficulties
    [SerializeField] GameObject levelPanel;      // Image/panel for levels

    [Header("Back Button")]
    [SerializeField] Button backToMenu;
    [SerializeField] Button backToDifficulty;

    void Start()
    {
        // IF EXIT BUTTON IS CLICKED
        if (backToMenu != null)
            backToMenu.onClick.AddListener(ExitToMenu);

        // BACK TO DIFFICULTY
        if (backToDifficulty != null)
            backToDifficulty.onClick.AddListener(BackToDifficulty);

        // CLEAR ALL FIRST
        foreach (Transform child in difficultyHolder)
        {
            Destroy(child.gameObject);
        }

        // Initially, show the difficulties panel and hide the levels panel.
        if (difficultyPanel != null)
            difficultyPanel.SetActive(true);
        if (levelPanel != null)
            levelPanel.SetActive(false);

        // For back buttons, use the GameObject reference for SetActive()
        if (backToMenu != null)
            backToMenu.gameObject.SetActive(true);
        if (backToDifficulty != null)
            backToDifficulty.gameObject.SetActive(false);

        FillDifficultyButtons();
    }

    void FillDifficultyButtons()
    {
        // CLEAR ALL FIRST
        foreach (Transform child in difficultyHolder)
        {
            Destroy(child.gameObject);
        }

        // FILL LEVEL
        foreach (var difficulty in allDifficulties)
        {
            GameObject newButton = Instantiate(difficultyPrefab, difficultyHolder, false);
            newButton.transform.Find("LevelSize").GetComponent<TMP_Text>().text = difficulty.levelSize;

            // Add OnClick Event to load levelButtons
            Button button = newButton.GetComponent<Button>();
            if(button != null )
            {
                button.onClick.AddListener(() => OnDifficultyClicked(difficulty));
            }
        }
    }

    void OnDifficultyClicked(SO_Difficulty difficulty)
    {
        // Hide the difficulties panel and show the levels panel.
        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);
        if (levelPanel != null)
            levelPanel.SetActive(true);

        // Toggle the back buttons: hide backToMenu, show backToDifficulty
        if (backToMenu != null)
            backToMenu.gameObject.SetActive(false);
        if (backToDifficulty != null)
            backToDifficulty.gameObject.SetActive(true);

        FillLevelButtons(difficulty);
    }

    void FillLevelButtons(SO_Difficulty difficulty)
    {
        // CLEAR ALL FIRST
        foreach (Transform child in levelHolder)
        {
            Destroy(child.gameObject);
        }

        // CREATE NEW BUTTON
        foreach (var level in difficulty.levels)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, levelHolder, false);
            levelButton.transform.Find("LevelName").GetComponent<TMP_Text>().text = level.levelName;

            // ADD ANOTHER ONCLICK EVENT TO LOAD THE CORRECT LEVEL DATA
            Button button = levelButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => LoadLevel(level));
            }
        }
    }

    void LoadLevel(SO_Level level)
    {
        NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(level.levelToLoad.text);
        LevelLoader.CurrentPuzzle = loadedPuzzle;

        // LOAD GAME AND DO STUFF
        SceneManager.LoadScene("GameScene");
    }

    void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void BackToDifficulty()
    {
        // Show the difficulties panel and hide the levels panel.
        if (difficultyPanel != null)
            difficultyPanel.SetActive(true);
        if (levelPanel != null)
            levelPanel.SetActive(false);

        // Toggle the back buttons: show backToMenu, hide backToDifficulty.
        if (backToMenu != null)
            backToMenu.gameObject.SetActive(true);
        if (backToDifficulty != null)
            backToDifficulty.gameObject.SetActive(false);
    }
}
