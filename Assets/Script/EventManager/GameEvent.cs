public enum GameEvent
{
    /// <summary>
    /// Used when the ball is launched though input
    /// </summary>
    LAUNCH_BALL,
    /// <summary>
    /// Used to add a score to the player
    /// </summary>
    PLAYER_SCORED,
    /// <summary>
    /// Player moved to new position
    /// </summary>
    POSITION_CHANGED,
    /// <summary>
    /// Game has started
    /// </summary>
    GAME_STARTED,
    /// <summary>
    /// Game is finished
    /// </summary>
    GAME_FINISHED,
    /// <summary>
    /// AI has launched the ball
    /// </summary>
    AI_LAUNCHED_BALL,
    /// <summary>
    /// AI scored a point
    /// </summary>
    AI_SCORED,
    /// <summary>
    /// AI moved to new position
    /// </summary>
    AI_POSITION_CHANGED,
    /// <summary>
    /// The fireball mode changed status
    /// </summary>
    FIREBALL_MODE_CHANGED,
    /// <summary>
    /// Player missed a shot
    /// </summary>
    MISSED_SHOT,
}
