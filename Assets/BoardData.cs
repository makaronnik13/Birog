using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<EncounterDeck, Queue<EcnounterCard>> _encounterDecks = new Dictionary<EncounterDeck, Queue<EcnounterCard>>();

    private Dictionary<Player, List<BattleCard>> _playersCards = new Dictionary<Player, List<BattleCard>>();

    private Queue<EventCard> _eventCards = new Queue<EventCard>();

    public EcnounterCard GetNextCard(EncounterDeck deck)
    {  
        return _encounterDecks[deck].Dequeue();
    }

    public void InitBoardData(List<List<EcnounterCard>> encounterCards, List<EventCard> eventCards, Dictionary<Player, List<BattleCard>> playersDeck)
    {
        
    }
}
