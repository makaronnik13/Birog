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

    public int LastEncounterDeckId;

    public int CurrentEncounter;

    private List<Queue<EncounterCard>> _encounterDecks = new List<Queue<EncounterCard>>();

    private Dictionary<Player, PlayerCards> _playersCards = new Dictionary<Player, PlayerCards>();

    private Queue<EventCard> _eventCards = new Queue<EventCard>();

    public EncounterCard GetNextEncounterCard(int deckId)
    {
        EncounterCard card = null;

        if (_encounterDecks[deckId].Count > 0)
        {
            card = _encounterDecks[deckId].Dequeue();
        }

        if (_encounterDecks[deckId].Count == 0)
        {
            Debug.LogWarning("NO cards in deck "+deckId);
        }
        return card;
    }

    public EventCard GetNextEventCard()
    {
        EventCard card = null;

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

    public List<BattleCard> TakeCards(Player player, int count)
    {
        List<BattleCard> takedCards = new List<BattleCard>();
        for (int i = 0; i<count; i++)
        {
            takedCards.Add(_playersCards[player].Deck.Dequeue());
        }
        _playersCards[player].Hand.AddRange(takedCards);
        return takedCards;
    }

    public void InitBoardData(EncounterDeck[] encounterDecks, EventsDeck eventsDeck, Dictionary<Player, List<BattleCard>> playersDeck)
    {
        foreach (KeyValuePair<Player, List<BattleCard>> pair in playersDeck)
        {
            PlayerCards cards = new PlayerCards();
            cards.Deck = new Queue<BattleCard>(pair.Value);
            _playersCards.Add(pair.Key, cards);
        }
        _eventCards = new Queue<EventCard>(eventsDeck.Cards);
        _encounterDecks = new List<Queue<EncounterCard>>();
        foreach (EncounterDeck deck in encounterDecks)
        {
            Queue<EncounterCard> encounterCards = new Queue<EncounterCard>(deck.cards.OrderBy(c=>Guid.NewGuid()));
            _encounterDecks.Add(encounterCards);
        }
    }
}
