using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Tooltip("the time in which the player will go to the next flag position after the ball was launched, consider the time the ball needs to reach the target!")]
    public float timeToNextPosition;

    [Tooltip("The time it takes to reach the basket since the ball was launched")]
    public float timeToLaunchBall;

    [Tooltip("Game time in seconds")]
    public int gameTime;
}
