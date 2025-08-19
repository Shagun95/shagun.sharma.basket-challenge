/// <summary>
/// This will be the main comunication system between script/components
/// changes are notified though our custom event system and data are handled
/// globally here
/// </summary>
public class SessionData : Singleton<SessionData>
{
    /// <summary>
    /// The shoot type currently achieved
    /// </summary>
    public ShootType currentShootType;
    /// <summary>
    /// Same for AI
    /// </summary>
    public ShootType AIcurrentShootType;
    public int scoreToAdd;
    public int AIScoreToAdd;
    /// <summary>
    /// The bonus set with the backboard shot
    /// </summary>
    public int currentTemporaryBonus = 0;
    public bool gameIsOn = false;
    /// <summary>
    /// Is the ball launching? used to disable the input (and the bar) while launching
    /// </summary>
    public bool ballIsLaunching = false;

    public int playerScoreForThisRound;
    public int AIScoreForThisRound;
    /// <summary>
    /// Used to calculate at what setting the launchbar should be (green and blue zones)
    /// </summary>
    public int currentShootPositionIndex;

    /// <summary>
    /// The distance between the arrow pointer and the chosen target (net or backboard)
    /// </summary>
    public float verticalDistance;

    public float AIVerticalDistance;

    public AI_LEVEL currentAILevel;

    public bool fireModeIsActive;
    
    public bool audioOn;
}
