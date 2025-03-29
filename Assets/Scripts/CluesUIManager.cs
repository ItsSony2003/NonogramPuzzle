using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CluesUIManager : MonoBehaviour
{
    public static CluesUIManager instance;
    [SerializeField] Transform rowCluesParents;
    [SerializeField] Transform colCluesParents;

    void Awake()
    {
        instance = this;
    }

    public void UpdateClues(NonogramPuzzle puzzle)
    {
        // Update Rows
        for (int r = 0; r < puzzle.RowClues.Length; r++)
        {
            string clueText = string.Join("", puzzle.RowClues[r].Clues);
            rowCluesParents.GetChild(r).GetComponent<TMP_Text>().text = clueText;
        }

        // Update Columns
        for (int c = 0; c < puzzle.ColClues.Length; c++)
        {
            string clueText = string.Join("\n", puzzle.ColClues[c].Clues);
            colCluesParents.GetChild(c).GetComponent<TMP_Text>().text = clueText;
        }
    }

}
