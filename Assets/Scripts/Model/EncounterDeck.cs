using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/EncounterDeck", order = 1)]
public class EncounterDeck : ScriptableObject
{
    public EncounterCard[] cards;
}
