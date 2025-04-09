using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] int rows = 6;
    [SerializeField] int columns = 6;

    // Grid
    [Header("Grid")]
    [SerializeField] Transform gridParent;
    [SerializeField] GameObject cellButtonPrefab;

    // Clue Transform
    [Header("Clue Transform")]
    [SerializeField] Transform rowClueParent;
    [SerializeField] Transform colClueParent;

    // Clue Prefab
    [Header("Clue Prefab")]
    [SerializeField] GameObject rowCluePrefab;
    [SerializeField] GameObject colCluePrefab;

    // Connection to a puzzle
    NonogramPuzzle puzzle;

    string filepath = Application.dataPath + "/Levels/";
    [SerializeField] TMP_InputField levelNameInput;

    void Awake()
    {
        instance = this;
    }


    // Creates a new puzzle, generates the grid, and sets the grid layout constraints
    void Start()
    {
        // New Puzzle
        puzzle = new NonogramPuzzle(rows, columns);

        // Generate Grid
        GenerateGrid();
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = columns;
    }

    // Clears the level name, creates a new puzzle, and regenerates the grid
    public void ResetPuzzle()
    {
        // New Puzzle
        levelNameInput.text = "";
        puzzle = new NonogramPuzzle(rows, columns);

        // Generate Grid
        GenerateGrid();
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = columns;
    }

    // Clears any existing grid and clue objects, then instantiates cell buttons and placeholder row/column clue
    void GenerateGrid()
    {
        // Clear all
        foreach(Transform child in gridParent)
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

        // Generate new Cell
        for(int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject cell = Instantiate(cellButtonPrefab, gridParent);
                cell.name = $"Cell ({r}, {c})";

                CellButton cellButton = cell.GetComponent<CellButton>();
                cellButton.row = r;
                cellButton.col = c;

                // Add the corresponding Puzzle
                cellButton.puzzle = puzzle;
            }
        }

        // Row Clue
        for (int r = 0; r < rows; r++)
        {
            GameObject rowClue = Instantiate(rowCluePrefab, rowClueParent);
            rowClue.name = $"Row {r}";
            rowClue.GetComponent<TMP_Text>().text = "0"; //New Placeholder Text
        }

        // Column Clue
        for (int c = 0; c < columns; c++)
        {
            GameObject colClue = Instantiate(colCluePrefab, colClueParent);
            colClue.name = $"Col {c}";
            colClue.GetComponent<TMP_Text>().text = "0"; //New Placeholder Text
        }
    }

    // Recalculates and updates the clues when a cell’s state changes
    public void OnCellStateChanged()
    {
        // Regenerate Clue
        GenerateClues();

        // Update Clue UI
        CluesUIManager.instance.UpdateClues(puzzle);
    }

    // Loops through each row/column of the solution grid to compute clue numbers using GetCluesForLine()
    void GenerateClues()
    {
        int rows = puzzle.SolutionData.GetLength(0);
        int columns = puzzle.SolutionData.GetLength(1);

        // Row Clues
        for (int r = 0; r < rows; r++)
        {
            puzzle.RowClues[r] = new CluesWrapper { Clues = GetCluesForLine(puzzle.SolutionData, r, true) };
        }

        // Column Clues
        for (int c = 0; c < columns; c++)
        {
            puzzle.ColClues[c] = new CluesWrapper { Clues = GetCluesForLine(puzzle.SolutionData, c, false) };
        }
    }

    // Scans a row or column for contiguous filled cells and returns a list of counts (or 0 if empty)
    List<int> GetCluesForLine(int[,] gridData, int index, bool isRow)
    {
        List<int> clues = new List<int>();
        int count = 0;

        // Gets the number of cells in the line: for a row, the number of columns; for a column, the number of rows
        int length = isRow ? gridData.GetLength(1) : gridData.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            int value = isRow ? gridData[index, i] : gridData[i, index];
            if(value == 1)
            {
                count++;
            }
            // If the cell is empty and count is positive, adds the current count to clues and resets count
            else if (count > 0)
            {
                clues.Add(count);
                count = 0;
            }
        }

        //if there is a left over clue
        // After the loop, if there’s a remaining block of filled cells(i.e.count > 0), adds it to clues
        if (count > 0)
        {
            clues.Add(count);
        }

        // If no clue found, add 0
        if(clues.Count == 0)
        {
            clues.Add(0);
        }

        return clues;
    }

    // ----- Saving and Loading Puzzle -----
    // Scans a row or column for contiguous filled cells and returns a list of counts (or 0 if empty)
    public void SavePuzzle() // Called from Button
    {
        if(string.IsNullOrEmpty(levelNameInput.text))
        {
            Debug.LogError("Inputfield is Empty");
            return;
        }
        puzzle.SolutionData = puzzle.SolutionData;
        string json = JsonUtility.ToJson(puzzle, true);
        string fullPath = filepath + levelNameInput.text + ".json";
        System.IO.File.WriteAllText(fullPath, json);
        Debug.Log($"Puzzle Saved to {fullPath}");
    }
}
