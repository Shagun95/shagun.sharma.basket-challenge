using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/AI Profiling")]
public class AIProfiling : ScriptableObject
{
    [BoxGroup("Level Easy")] 
    public DifficultyProfile Easy;
    
    [BoxGroup("Level Medium")]
    public DifficultyProfile Medium;
    
    [BoxGroup("Level Hard")]
    public DifficultyProfile Hard;
    
    [BoxGroup("Level Legend")]
    public DifficultyProfile Legend;

    [BoxGroup("Genaral")]
    public float minShootTime, maxShootTime;
}
