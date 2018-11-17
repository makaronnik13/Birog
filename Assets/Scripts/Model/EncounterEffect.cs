using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public class EncounterEffect
{
    public enum EffectPersonAim
    {
        Player,
        Others,
        All
    }

    public enum EffectDestenationAim
    {
        Hand,
        Deck,
        Drop,
        All
    }

    public enum EffectType
    {
        ChangeHp,
        ChangeTime,
        ChangeArmor,
        ChangeMaxHp,
        DrawCards,
        EndOfTurn,
        DropRandomCards,
        BurnRandomCards,
        DropRandomCardsByRes,
        BurnRandomCardsByRes,
        DropRandomCardsByType,
        BurnRandomCardsByType,
        TakeCards,
        TakeRandomCards,
        TakeCardsWithChoose,
        AddCardsToDeck,
        RemoveCardsFromDeck
    }

    [HorizontalGroup("Group 1", LabelWidth = 0)]
    [HideLabel]
    public EffectType Effect;

    [ShowIf("ShowPerson")]
    [HideLabel]
    [HorizontalGroup("Group 1", LabelWidth = 0)]
    public EffectPersonAim PersonAim;

    [ShowIf("ShowDest")]
    [HideLabel]
    [HorizontalGroup("Group 1", LabelWidth = 0)]
    public EffectDestenationAim DestenationAim;

    [ShowIf("ShowValue")]
    public int Value;

    [ShowIf("ShowBattleCards")]
    public List<BattleCard> BattleCards;

    [ShowIf("ShowEncounterCards")]
    public List<EncounterCard> EncounterCards;

    [ShowIf("ShowResources")]
    public List<ResourcePair> Resources;

    [ShowIf("ShowCardTypes")]
    public List<CardStats.CardType> CardTypes;

    [ShowIf("ShowChooseCount")]
    public int ChooseCount;
    [ShowIf("ShowCardsCount")]
    public int CardsCount;

    private bool ShowPerson()
    {
        return Effect != EffectType.ChangeTime && Effect != EffectType.AddCardsToDeck && Effect != EffectType.RemoveCardsFromDeck && Effect != EffectType.EndOfTurn && 
            Effect != EffectType.TakeCardsWithChoose;
    }
    private bool ShowDest()
    {
        return Effect == EffectType.BurnRandomCards || Effect == EffectType.BurnRandomCardsByRes || Effect == EffectType.BurnRandomCardsByType;
    }
    private bool ShowValue()
    {
        return Effect == EffectType.ChangeArmor || Effect == EffectType.ChangeHp || Effect == EffectType.ChangeMaxHp || Effect == EffectType.ChangeTime;
    }
    private bool ShowBattleCards()
    {
        return Effect == EffectType.TakeCards || Effect == EffectType.TakeCardsWithChoose || Effect == EffectType.TakeRandomCards;
    }
    private bool ShowEncounterCards()
    {
        return Effect == EffectType.AddCardsToDeck || Effect == EffectType.RemoveCardsFromDeck;
    }
    private bool ShowResources()
    {
        return Effect == EffectType.BurnRandomCardsByRes || Effect == EffectType.DropRandomCardsByRes;
    }
    private bool ShowCardTypes()
    {
        return Effect == EffectType.BurnRandomCardsByType || Effect == EffectType.DropRandomCardsByType;
    }
    private bool ShowChooseCount()
    {
        return Effect == EffectType.TakeCardsWithChoose;
    }

    private bool ShowCardsCount()
    {
        return Effect == EffectType.TakeCardsWithChoose || Effect == EffectType.TakeRandomCards || Effect == EffectType.BurnRandomCards || Effect == EffectType.BurnRandomCardsByRes || Effect == EffectType.BurnRandomCardsByType;
    }
}
