using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/BattleCard", order = 1)]
public class BattleCard : ScriptableObject, ICard
{
    [SerializeField]
    public string CardName;
    [SerializeField]
    public Sprite Image;
    [SerializeField]
    public string Description;


    public CardStats.CardType CardType;
    public List<ResourcePair> Resources;

    public string CardDescription()
    {
        return Description;
    }

    public Sprite CardImage()
    {
        return Image;
    }

    string ICard.CardName()
    {
        return CardName;
    }
}

public class CardStats
{
    public enum Resources
    {
        Knowlege,
        Faith,
        Force,
        Loot,
        Skill
    }

    public enum CardType
    {
       Spell,
       Gun,
       Cloth,
       Minion,
       Skill,
       Tools
    }
}
