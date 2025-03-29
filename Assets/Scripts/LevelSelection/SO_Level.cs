using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Level", menuName ="Nonograms/Level")]
public class SO_Level : ScriptableObject
{
    public string levelName;
    public string levelSize; //5x5, 6x6
    public TextAsset levelToLoad;

    private void OnValidate()
    {
        if(levelToLoad != null)
        {
            NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(levelToLoad.text);
            if(loadedPuzzle != null)
            {
                levelName = levelToLoad.name;
                levelSize = $"{loadedPuzzle.Cols} x {loadedPuzzle.Rows}";
            }
        }
    }
}
