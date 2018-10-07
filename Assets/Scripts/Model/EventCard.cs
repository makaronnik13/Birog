using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/EventCard", order = 1)]
public class EventCard : ScriptableObject, ICard
{
    public string CardName;
    public Sprite Image;
    public string Description;

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
