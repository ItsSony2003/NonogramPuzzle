using System.Collections;
using System.Collections.Generic;
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

        for (int i = 0; i < rows; i++)
        {
            RowClues[i] = new CluesWrapper();
        }

        for (int i = 0; i < cols; i++)
        {
            ColClues[i] = new CluesWrapper();
        }

        // Init Empty Grid
        GridData = new int[rows, cols];
        SolutionData = new int[rows, cols];
    }

    [System.NonSerialized]
    int[,] gridData;
    [System.NonSerialized]
    int[,] solutionData;

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

    public int[,] SolutionData
    {
        get
        {
            if (solutionData == null)
            {
                solutionData = new int[Rows, Cols];
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
