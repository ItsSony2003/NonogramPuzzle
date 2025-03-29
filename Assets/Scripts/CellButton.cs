using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    Empty,
    Filled,
    Crossed,
    Unknown
}
public class CellButton : MonoBehaviour
{
    public CellState State { get; private set; } = CellState.Empty;

    [SerializeField] Sprite emptySprite, filledSprite, crossedSprite, unknownSprite;

    [HideInInspector] public int row;
    [HideInInspector] public int col;

    // CONNECTION TO PUZZLE
    [HideInInspector] public NonogramPuzzle puzzle;

    public void ChangeGeneratorState() // Called from the button
    {
        if (State == CellState.Empty)
        {
            State = CellState.Filled;
        }
        else
        {
            State = CellState.Empty;
        }

        puzzle.SolutionData[row, col] = puzzle.SolutionData[row, col] == 1 ? 0 : 1;

        UpdateVisual();

        //Notify GridManager
        GridManager.instance.OnCellStateChanged();
    }

    public void ChangeGameState()
    {
        if (State == CellState.Empty)
        {
            State = CellState.Filled;
            puzzle.GridData[row, col] = 1;
        }
        else if (State == CellState.Filled)
        {
            State = CellState.Unknown;
            puzzle.GridData[row, col] = 2;
        }
        else
        {
            State = CellState.Empty;
            puzzle.GridData[row, col] = 0;
        }
        UpdateVisual();

        // Check if the game is over (win condition)
        GameManager.Instance.CheckWin();
    }    

    private void UpdateVisual()
    {
        switch(State)
        {
            case CellState.Empty:
                GetComponent<Button>().image.sprite = emptySprite;
                break;
            case CellState.Filled:
                GetComponent<Button>().image.sprite = filledSprite;
                break;
            case CellState.Crossed:
                GetComponent<Button>().image.sprite = crossedSprite;
                break;
            case CellState.Unknown:
                GetComponent<Button>().image.sprite = unknownSprite;
                break;
        }
    }
}
