using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    [Required]
    public GameSettings gameSettings;

    [Required]
    public RandomBonusSettings randomBonusSettings;
}
