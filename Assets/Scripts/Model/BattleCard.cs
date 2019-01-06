using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/BattleCard", order = 1)]
public class BattleCard : ScriptableObject, ICard
{
    public int Id;

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
        Tooth,
        Blood,
        Shroom,
        Mutations,
        Parasite
    }

    public enum CardType
    {
       Weapon,
       Mutation,
       Fint,
       Minion
    }
}
