using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Tooltip("the time in which the player will go to the next flag position after the ball was launched, consider the time the ball needs to reach the target!")]
    public float TimeToNextPosition;

    [Tooltip("The time it takes to reach the basket since the ball was launched")]
    public float TimeToLaunchBall;

    [Tooltip("Game time in seconds")]
    public int GameTime;
    
    [Tooltip("The time in which the launch bar will launch the ball automatically")]
    public float BarTimer;
}
