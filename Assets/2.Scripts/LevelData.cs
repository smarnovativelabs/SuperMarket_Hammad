using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable/LevelData")]
public class LevelData : ScriptableObject
{

    public int level;
    public int xpRequired;
    public int blitzReward;
    public int cashReward;
   // public List<LicenseReward> licenses; // List of licenses for this level
}
