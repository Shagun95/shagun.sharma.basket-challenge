using Sirenix.OdinInspector;

/// <summary>
/// These are the main settings reference of the game, easily
/// accessible globally
/// </summary>
public class GameData : Singleton<GameData>
{
    [Required]
    public GameSettings gameSettings;

    [Required]
    public RandomBonusSettings randomBonusSettings;

    [Required]
    public AIProfiling AIProfiling;
}
