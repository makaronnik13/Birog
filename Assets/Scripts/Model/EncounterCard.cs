using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/EncounterCard", order = 1)]
public class EcnounterCard : ScriptableObject, ICard
{
    public string CardName;
    public Sprite Image;
    public string Description;
    public List<EncounterVariant> Variants = new List<EncounterVariant>();

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
