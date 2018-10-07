using System;
using System.Collections;
using System.Collections.Generic;
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

    public CardsLayout Nowhere, PlayerHand, PlayerDeck, PlayerDrop, EventDeck, CurrentEncounter, EventDrop;
    public CardsLayout[] EncounterDecks;

    private void Start()
    {
        
    }

    public CardBehaviour CreateCardIn(BattleCard card, SlotType slot = SlotType.Nowhere, int additionalId = -1)
    {
        GameObject cardGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.BattleCard));
        cardGo.GetComponent<BattleCardVisual>().Init(card, true);
        return CardCreation(cardGo, slot, additionalId);
    }

    public CardBehaviour CreateCardIn(EncounterCard card, SlotType slot = SlotType.Nowhere, int additionalId = -1)
    {
        GameObject cardGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.EncounterCard));
        cardGo.GetComponent<EncounterCardVisual>().Init(card, true);
        return CardCreation(cardGo, slot, additionalId);
    }

    public CardBehaviour CreateCardIn(EventCard card, SlotType slot = SlotType.Nowhere)
    {
        GameObject cardGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.EventCard));
        cardGo.GetComponent<EventCardVisual>().Init(card, true);
        return CardCreation(cardGo, slot, -1);
    }

    private CardBehaviour CardCreation(GameObject cardGo, SlotType slot = SlotType.Nowhere, int additionalId = -1)
    {
        CardsLayout aimLayout = GetLayout(slot, additionalId);
        cardGo.transform.SetParent(aimLayout.transform);
        cardGo.transform.localScale = Vector3.one;
        cardGo.transform.localPosition = Vector3.zero;
        cardGo.transform.localRotation = Quaternion.identity;
        CardBehaviour cardBehasviour = cardGo.GetComponent<CardBehaviour>();
        return cardBehasviour;
    }

    public void MoveCardTo(CardBehaviour cardBehaviour, SlotType aim, int id = -1, Action callback = null)
    {
        GetLayout(aim, id).AddCardToLayout(cardBehaviour, callback);
    }

    private CardsLayout GetLayout(SlotType slot, int id = -1)
    {
        switch (slot)
        {
            case SlotType.EncounterDeck:
                Debug.Log(id);
                return EncounterDecks[id-15];
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

    public void HideEncounterCard(int deckId)
    {
        EncounterCardVisual cv = CurrentEncounter.GetComponentInChildren<EncounterCardVisual>();
        cv.HideVariants(()=>
        {
            MoveCardTo(cv.GetComponent<CardBehaviour>(), SlotType.EncounterDeck, deckId, ()=> {
                Destroy(cv.gameObject);
            });
        });
    }
}
