using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Create different profiles for more levels of AI
/// </summary>
[System.Serializable]
public class DifficultyProfile
{
    /// <summary>
    /// The precision in percentage
    /// </summary>
    [Range(0, 100), SuffixLabel("%", Overlay = true)]
    public int Precision;

    /// <summary>
    /// The chance to have a ring shot, in percentage
    /// </summary>
    [Range(0, 100), SuffixLabel("%", Overlay = true)]
    public int RingChance;

    /// <summary>
    /// The chance to have a backboard shot, in percentage
    /// </summary>
    [Range(0, 100), SuffixLabel("%", Overlay = true)]
    public int BackBoardChance;
}
