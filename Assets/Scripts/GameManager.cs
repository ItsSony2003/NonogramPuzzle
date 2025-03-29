using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [SerializeField] List<TextAsset> puzzleFiles;
    [SerializeField] Transform gridParent;
    [SerializeField] Transform rowClueParent;
    [SerializeField] Transform colClueParent;

    [SerializeField] GameObject rowCluePrefab;
    [SerializeField] GameObject colCluePrefab;
    [SerializeField] GameObject cellGamePrefab;

    [Header("Win")]
    [SerializeField] GameObject winPanel;
    [SerializeField] Button exitButton;
    [SerializeField] Button exit2Button;

    int puzzleIndex = 0;
    int rows, columns = 0;

    NonogramPuzzle puzzle;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(LevelSelect);
        exit2Button.onClick.AddListener(LevelSelect);

        if (winPanel != null)
            winPanel.SetActive(false);
        SetPuzzleIndex();
    }

    void SetPuzzleIndex()
    {
        // After set puzzle


        // Load Correct Puzzle
        LoadCurrentPuzzle();
    }

    void LoadCurrentPuzzle()
    {
        puzzle = LoadPuzzle();
        if (puzzle != null)
        {
            // Generate New Puzzle
            GeneratePuzzle();
        }
    }

    NonogramPuzzle LoadPuzzle()
    {
        if(LevelLoader.CurrentPuzzle != null)
        {
            return LevelLoader.CurrentPuzzle;
        }
        TextAsset selectedPuzzle = puzzleFiles[puzzleIndex];
        string json = selectedPuzzle.text;
        NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(json);
        return loadedPuzzle;
    }

    void GeneratePuzzle()
    {
        rows = puzzle.Rows;
        columns = puzzle.Cols;

        gridParent.GetComponent<GridLayoutGroup>().constraintCount = columns;


        // Clear Existing Clue
        ClearCluesInGrid();

        // Create Row Clues
        GenerateRowClues();

        // Create Column Clues
        GenerateColClues();

        // Generate Cell
        GenerateCellButton();
    }

    void ClearCluesInGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rowClueParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in colClueParent)
        {
            Destroy(child.gameObject);
        }
    }

    void GenerateRowClues()
    {
        for (int r = 0; r < rows; r++)
        {
            GameObject rowClue = Instantiate(rowCluePrefab, rowClueParent);
            rowClue.GetComponentInChildren<TMP_Text>().text = string.Join("", puzzle.RowClues[r].Clues);
        }
    }

    void GenerateColClues()
    {
        for (int c = 0; c < columns; c++)
        {
            GameObject colClue = Instantiate(colCluePrefab, colClueParent);
            colClue.GetComponentInChildren<TMP_Text>().text = string.Join("\n", puzzle.ColClues[c].Clues);
        }
    }

    void GenerateCellButton()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject cell = Instantiate(cellGamePrefab, gridParent);
                CellButton cellButton = cell.GetComponent<CellButton>();

                cellButton.row = r;
                cellButton.col = c;

                // Add the corresponding Puzzle
                cellButton.puzzle = puzzle;
            }
        }
    }

    public void CheckWin()
    {
        for (int r = 0; r < puzzle.Rows; r++)
        {
            for (int c = 0; c < puzzle.Cols; c++)
            {
                if (puzzle.SolutionData[r, c] == 1 && puzzle.GridData[r, c] != 1 || puzzle.SolutionData[r, c] == 0 && puzzle.GridData[r, c] == 1)
                {
                    // Puzzle Not Solved
                    Debug.Log("Not Solved Yet!");
                    return;
                }
            }
        }

        // Puzzle Solved (Win)
        // Show Win Scene
        Debug.Log("Game Won");
        if(winPanel != null)
            winPanel.SetActive(true);
    }

    void LevelSelect()
    {
        SceneManager.LoadScene("LoadLevel");
    }
}
