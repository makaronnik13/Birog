using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventCardVisual : CardVisual
{
    public void Init(EventCardWrapper card, bool show)
    {
        base.Init(card.Card, card.Guid);    
    }
}
