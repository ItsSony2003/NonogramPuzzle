using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficuly", menuName = "Nonograms/Difficulty")]
public class SO_Difficulty : ScriptableObject
{
    public string levelSize;
    public SO_Level[] levels;
}
