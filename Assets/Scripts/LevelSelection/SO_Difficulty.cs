using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficuly", menuName = "Nonograms/Difficulty")]

//  defines a Nonogram difficulty level. It stores a descriptor (like "5x5" or "6x6")
//  and an array of level assets (SO_Level) grouped by that difficulty
public class SO_Difficulty : ScriptableObject
{
    public string levelSize;
    public SO_Level[] levels;
}
