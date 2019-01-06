using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardData : MonoBehaviour
{
    private static BoardData _instance;
    public static BoardData Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<BoardData>();
            }
            return _instance;
        }
    }

    public int CurrentEncounter;

    private Queue<EncounterCardWrapper> _encounterDeck = new Queue<EncounterCardWrapper>();

    private List<PlayerData> _playersData = new List<PlayerData>();

    public PlayerData GetPlayer(Player player)
    {
        return _playersData.FirstOrDefault(p=>p.player == player);
    }

    private Queue<EventCardWrapper> _eventCards = new Queue<EventCardWrapper>();

    public EncounterCardWrapper GetNextEncounterCard()
    {
        EncounterCardWrapper card = null;

        if (_encounterDeck.Count > 0)
        {
            card = _encounterDeck.Dequeue();
        }

        if (_encounterDeck.Count == 0)
        {
            Debug.LogWarning("NO cards in deck");
        }
        return card;
    }

    public EventCardWrapper GetNextEventCard()
    {
        EventCardWrapper card = null;

        if (_eventCards.Count > 0)
        {
            card = _eventCards.Dequeue();
        }

        if (_eventCards.Count == 0)
        {
            Debug.LogWarning("NO cards in event deck");
        }

        return card;
    }

    public List<BattleCardWrapper> TakeCards(Player player, int count)
    {
        List<BattleCardWrapper> takedCards = new List<BattleCardWrapper>();
        for (int i = 0; i<count; i++)
        {
            if (_playersData.FirstOrDefault(p => p.player == player).Deck.Count == 0)
            {
                _playersData.FirstOrDefault(p => p.player == player).UpdateDeck();
            }

            if (_playersData.FirstOrDefault(p => p.player == player).Deck.Count>0)
            {
                takedCards.Add(_playersData.FirstOrDefault(p => p.player == player).Deck.Dequeue());
            }
        }
        _playersData.FirstOrDefault(p => p.player == player).Hand.AddRange(takedCards);

        return takedCards;
    }

    public void InitBoardData(EncounterCard[] encounterDeck, EventCard[] eventsDeck)
    {
        List<EventCardWrapper> eventCards = new List<EventCardWrapper>();
        List<EncounterCardWrapper> encounterCards = new List<EncounterCardWrapper>();
       
        foreach (EncounterCard card in encounterDeck)
        {
            encounterCards.Add(new EncounterCardWrapper(card));
        }

        foreach (EventCard card in eventsDeck)
        {
            eventCards.Add(new EventCardWrapper(card));
        }

        _eventCards = new Queue<EventCardWrapper>(eventCards);
        _encounterDeck = new Queue<EncounterCardWrapper>(encounterCards);
    }

    public void AddPlayer(Player player, List<BattleCard> cards, int hp, int armor, int initiative)
    {
        _playersData.Add(new PlayerData(player, cards, hp, armor, initiative));
    }
}
