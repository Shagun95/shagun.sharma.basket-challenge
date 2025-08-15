/// <summary>
/// Type of postion to setup the player (2 points, 3 points etc) they also have
/// different setup of the bar
/// </summary>
public enum Position
{
    LAUNCH_ONE,
    LAUNCH_TWO,
    LAUNCH_THREE
}

/// <summary>
/// Used for detecting score
/// </summary>
public enum ShootType 
{
    /// <summary>
    /// Perfect score
    /// </summary>
    NET,
    /// <summary>
    /// Touching the ring
    /// </summary>
    RING,
    /// <summary>
    /// Backboard bonus
    /// </summary>
    BACK_BOARD
}

public enum BallOwner
{
    Player,
    AI
}