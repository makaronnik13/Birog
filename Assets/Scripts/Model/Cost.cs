using Malee;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[System.Serializable]
public class Cost
{
    public enum CostEffect
    {
        Drop,
        Burn
    }

    public enum CostType
    {
        Resource,
        Type,
        Card
    }

    [HideLabel]
    [HorizontalGroup("Group 1"), LabelWidth(0)]
    public CostEffect Effect;

    [HideLabel]
    [HorizontalGroup("Group 1"), LabelWidth(0)]
    public CostType Type;

    [ShowIf("ShowRes")]
    public List<CardStats.Resources> Resources;
    [ShowIf("ShowTypes")]
    public List<CardStats.CardType> CardTypes;
    [ShowIf("ShowCards")]
    public List<BattleCard> Cards;


    private bool ShowRes()
    {
        return Type == CostType.Resource;
    }

    private bool ShowTypes()
    {
        return Type == CostType.Type;
    }

    private bool ShowCards()
    {
        return Type == CostType.Card;
    }
}