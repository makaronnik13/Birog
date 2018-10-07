using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Quest", order = 1)]

public class Quest : ScriptableObject
{
    public string QuestName;
    public string QuestDescription;
    public Sprite QuestImage;
    public EncounterDeck[] EncounterDecks;
    public EventsDeck EventsDeck;
}
