using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "RandomBonusSettings", menuName = "Settings/Random Bonus Settings")]
public class RandomBonusSettings : ScriptableObject
{
    [BoxGroup("Bonus chance", centerLabel: true)]
    [LabelText("(%)")]
    [Range(0, 100)]
    public int randomBonusRate;

   
    [BoxGroup("Bonus & Chance", centerLabel: true)]
    [HorizontalGroup("Bonus & Chance/Common")]
    [LabelText("Common")]
    public int commonBonus;

    [HorizontalGroup("Bonus & Chance/Common"), LabelWidth(30)]
    [LabelText("%")]
    [Range(0, 100)]
    public int chanceCommonBonus;

    [HorizontalGroup("Bonus & Chance/Rare")]
    [LabelText("Rare")]
    public int rareBonus;

    [HorizontalGroup("Bonus & Chance/Rare"), LabelWidth(30)]
    [LabelText("%")]
    [Range(0, 100)]
    public int chanceRareBonus;

    [HorizontalGroup("Bonus & Chance/VeryRare")]
    [LabelText("Very Rare")]
    public int veryRareBonus;

    [HorizontalGroup("Bonus & Chance/VeryRare"), LabelWidth(30)]
    [LabelText("%")]
    [Range(0, 100)]
    public int chanceVeryRareBonus;

    /// <summary>
    /// Should the bonus be activated
    /// </summary>
    /// <returns></returns>
    public bool BonusActive()
    {
        return Random.Range(0, 100) < randomBonusRate;
    }

    /// <summary>
    /// Gives the value of the bonus, depending on its "chance"
    /// </summary>
    /// <returns></returns>
    public int GetRandomBonus()
    {
        int roll = Random.Range(0, 100);
        
        if (roll < chanceCommonBonus)
            return commonBonus;
        
        if (roll < chanceCommonBonus + chanceRareBonus)
            return rareBonus;
        
        return veryRareBonus;
    }
}
