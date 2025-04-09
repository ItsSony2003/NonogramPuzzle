using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CluesWrapper
{
    public List<int> Clues = new List<int>();
}

[System.Serializable]
public class NonogramPuzzle 
{
    public int Rows;
    public int Cols;
    public CluesWrapper[] RowClues;
    public CluesWrapper[] ColClues;

    public int[] SolutionFlatGridData; // 1D array, for serialization

    public NonogramPuzzle(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;

        RowClues = new CluesWrapper[rows];
        ColClues = new CluesWrapper[cols];

        // Creates and initializes arrays for row and column clue
        for (int i = 0; i < rows; i++)
        {
            RowClues[i] = new CluesWrapper();
        }

        for (int i = 0; i < cols; i++)
        {
            ColClues[i] = new CluesWrapper();
        }

        // Init Empty Grid for both the player's grid (GridData) and the correct solution (SolutionData)
        GridData = new int[rows, cols];
        SolutionData = new int[rows, cols];
    }

    [System.NonSerialized]
    int[,] gridData;
    [System.NonSerialized]
    int[,] solutionData;


    // creates and returns a 2D grid of size Rows×Cols
    public int[,] GridData
    {
        get
        {
            if(gridData == null)
            {
                // init(ialize) empty grid
                gridData = new int[Rows, Cols];
            }
            return gridData;
        }
        set
        {
            gridData = value;
        }
    }


    // gets/sets the 2D solution grid. On get, it lazily builds the grid from a 1D flattened array
    // on set, it updates the flattened array to match the new grid
    public int[,] SolutionData
    {
        get
        {
            // If solutionData is null, it creates a new 2D array of size Rows×Cols
            if (solutionData == null)
            {
                solutionData = new int[Rows, Cols];
                // It then fills each cell by converting the 1D index from SolutionFlatGridData into 2D indices (using r * Cols + c)
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        SolutionData[r, c] = SolutionFlatGridData[r * Cols + c];
                    }
                }
            }
            return solutionData;
        }
        set
        {
            // 1. Assigns the provided 2D array to solutionData
            // 2. Rebuilds the flattened array (SolutionFlatGridData) by iterating over the 2D array
            // and storing each value at the corresponding 1D index (r * Cols + c)
            solutionData = value;
            SolutionFlatGridData = new int[Rows * Cols];
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    SolutionFlatGridData[r * Cols + c] = value[r, c];
                }
            }
        }
    }
}
