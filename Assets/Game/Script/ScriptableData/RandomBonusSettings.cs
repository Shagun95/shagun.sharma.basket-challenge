using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "RandomBonusSettings", menuName = "Settings/Random Bonus Settings")]
public class RandomBonusSettings : ScriptableObject
{
    [FormerlySerializedAs("randomBonusRate")]
    [BoxGroup("Bonus chance", centerLabel: true)]
    [LabelText("(%)")]
    [Range(0, 100)]
    public int RandomBonusRate;

   
    [FormerlySerializedAs("commonBonus")]
    [BoxGroup("Bonus & Chance", centerLabel: true)]
    [HorizontalGroup("Bonus & Chance/Common")]
    [LabelText("Common")]
    public int CommonBonus;

    [FormerlySerializedAs("chanceCommonBonus")]
    [HorizontalGroup("Bonus & Chance/Common"), LabelWidth(30)]
    [LabelText("%")]
    [Range(0, 100)]
    public int ChanceCommonBonus;

    [FormerlySerializedAs("rareBonus")]
    [HorizontalGroup("Bonus & Chance/Rare")]
    [LabelText("Rare")]
    public int RareBonus;

    [FormerlySerializedAs("chanceRareBonus")]
    [HorizontalGroup("Bonus & Chance/Rare"), LabelWidth(30)]
    [LabelText("%")]
    [Range(0, 100)]
    public int ChanceRareBonus;

    [FormerlySerializedAs("veryRareBonus")]
    [HorizontalGroup("Bonus & Chance/VeryRare")]
    [LabelText("Very Rare")]
    public int VeryRareBonus;

    [FormerlySerializedAs("chanceVeryRareBonus")]
    [HorizontalGroup("Bonus & Chance/VeryRare"), LabelWidth(30)]
    [LabelText("%")]
    [Range(0, 100)]
    public int ChanceVeryRareBonus;

    /// <summary>
    /// Should the bonus be activated
    /// </summary>
    /// <returns></returns>
    public bool BonusActive()
    {
        return Random.Range(0, 100) < RandomBonusRate;
    }

    /// <summary>
    /// Gives the value of the bonus, depending on its "chance"
    /// </summary>
    /// <returns></returns>
    public int GetRandomBonus()
    {
        int roll = Random.Range(0, 100);
        
        if (roll < ChanceCommonBonus)
            return CommonBonus;
        
        if (roll < ChanceCommonBonus + ChanceRareBonus)
            return RareBonus;
        
        return VeryRareBonus;
    }
}
