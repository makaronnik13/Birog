using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsLayoutManager : Singleton<CardsLayoutManager> {

    public enum SlotType 
    {
        Nowhere,
        PlayerHand,
        PlayerDeck,
        PlayerDrop,
        EncounterDeck,
        EventDeck,
        CurrentEncounter,
        EventDrop
    }

    public CardsLayout Nowhere, PlayerHand, PlayerDeck, PlayerDrop, EventDeck, CurrentEncounter, EventDrop, EncounterDeck;

    private void Start()
    {
        
    }

    public CardBehaviour CreateCardIn(BattleCardWrapper card, SlotType slot = SlotType.Nowhere)
    {
        Debug.Log("instant");
        GameObject cardGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.BattleCard));
        cardGo.GetComponent<BattleCardVisual>().Init(card, true);
        return CardCreation(cardGo, slot);
    }

    public CardBehaviour CreateCardIn(EncounterCardWrapper card, SlotType slot = SlotType.Nowhere)
    {
        GameObject cardGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.EncounterCard));
        cardGo.GetComponent<EncounterCardVisual>().Init(card, true);
        return CardCreation(cardGo,slot);
    }

    public CardBehaviour CreateCardIn(EventCardWrapper card, SlotType slot = SlotType.Nowhere)
    {
        GameObject cardGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.EventCard));
        cardGo.GetComponent<EventCardVisual>().Init(card, true);
        return CardCreation(cardGo, slot);
    }

    private CardBehaviour CardCreation(GameObject cardGo, SlotType slot = SlotType.Nowhere)
    {
        CardsLayout aimLayout = GetLayout(slot);
        cardGo.transform.SetParent(aimLayout.transform);
        cardGo.transform.localScale = Vector3.one;
        cardGo.transform.localPosition = Vector3.zero;
        cardGo.transform.localRotation = Quaternion.identity;
        CardBehaviour cardBehasviour = cardGo.GetComponent<CardBehaviour>();
        return cardBehasviour;
    }

    public void MoveCardTo(CardBehaviour cardBehaviour, SlotType aim, Action callback = null, bool withoutDelay = false)
    {
        GetLayout(aim).AddCardToLayout(cardBehaviour);
        cardBehaviour.AddCallback(callback);

        if (withoutDelay)
        {
            cardBehaviour.transform.localPosition = cardBehaviour.GetPosition(GetLayout(aim));
            cardBehaviour.transform.localRotation = cardBehaviour.GetRotation(GetLayout(aim));
        }
    }

    private CardsLayout GetLayout(SlotType slot)
    {
        switch (slot)
        {
            case SlotType.EncounterDeck:
                return EncounterDeck;
            case SlotType.EventDeck:
                return EventDeck;
            case SlotType.Nowhere:
                return Nowhere;
            case SlotType.PlayerDeck:
                return PlayerDeck;
            case SlotType.PlayerDrop:
                return PlayerDrop;
            case SlotType.PlayerHand:
                return PlayerHand;
            case SlotType.CurrentEncounter:
                return CurrentEncounter;
            case SlotType.EventDrop:
                return EventDrop;
        }

        return Nowhere;
    }

    public void HideEncounterCard()
    {
        EncounterCardVisual cv = CurrentEncounter.GetComponentInChildren<EncounterCardVisual>();
        cv.HideVariants(()=>
        {
            MoveCardTo(cv.GetComponent<CardBehaviour>(), SlotType.EncounterDeck, ()=> {
                Destroy(cv.gameObject);
            });
        });
    }

    public CardBehaviour GetCardFrom(BattleCard card, SlotType playerDeck)
    {
        return GetLayout(playerDeck).Cards.FirstOrDefault(c=>(ScriptableObject)c.GetComponent<CardVisual>()._card == card);
    }
}
