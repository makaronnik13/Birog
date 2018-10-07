using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/EventsDeck", order = 1)]
public class EventsDeck : ScriptableObject
{
    public EventCard[] Cards;
}
