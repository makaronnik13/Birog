using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DefaultResources
{
    //player custom properties keys
    public static string PLAYER_LOADED_LEVEL = "LevelLoaded";
    public static string PLAYER_CLASS = "PlayerClass";

    public static string PLAYER_LIVES = "PlayerLives";
    public static string PLAYER_CHOOSED_CARDS = "PlayerFinishedTurn";
    public static string PLAYER_CHOOSED_CARDS_TO_ATACK = "PlayerChoosedCardsToAtack";
    public static string PLAYER_HAND = "PlayerHand";
    public static string PLAYER_DECK = "PlayerDeck";
    public static string PLAYER_DROP = "PlayerDrop";

    //events codes
    public static byte START_GAME_EVENT = 0;


    private static object[] _allCards = null;
    public static object[] AllCards
    {
        get
        {
            if (_allCards == null)
            {
                EcnounterCard[] encounterCards  = Resources.LoadAll<EcnounterCard>("Cards");
                BattleCard[] battleCards = Resources.LoadAll<BattleCard>("Cards");
                EventCard[] eventsCards = Resources.LoadAll<EventCard>("Cards");

                List<object> cards = new List<object>();
                cards.AddRange(encounterCards);
                cards.AddRange(battleCards);
                cards.AddRange(eventsCards);
                _allCards = cards.ToArray();
            }
            return _allCards;
        }
    }

    private static BattlerClass[] _allClasses = null;
    public static BattlerClass[] AllClasses
    {
        get
        {
            if (_allClasses == null)
            {
                _allClasses = Resources.LoadAll<BattlerClass>("Classes");
            }
            return _allClasses;
        }
    }
    public static BattlerClass GetClassById(int id)
    {
        return AllClasses[id];
    }

    public static int GetCardId(ICard card)
    {
        return AllCards.ToList().IndexOf(card);
    }

    public static ICard GetCardById(int i)
    {
        return ((ICard)AllCards[i]);
    }


    public enum PrefabType
    {
        BattleCard,
        EventCard,
        EncounterCard,
        ResourceIcon
    }

    private static List<GameObject> _prefabs = new List<GameObject>();
    private static List<GameObject> prefabs
    {
        get
        {
            if (_prefabs.Count == 0)
            {
                _prefabs = Resources.LoadAll<GameObject>("").ToList();
            }
            return _prefabs;
        }
    }

    public static GameObject GetPrefab(PrefabType prefabType)
    {
        return prefabs[(int)prefabType];
    }

    private static Sprite[] _resourcesSprites = null;
    private static Sprite[] resourcesSprites
    {
        get
        {
            if (_resourcesSprites == null)
            {
                _resourcesSprites = Resources.LoadAll<Sprite>("Sprites/ResourcesIcons");
            }
            return _resourcesSprites;
        }
    }
    public static Sprite GetResourceSprite(CardStats.Resources res)
    {
        return resourcesSprites[(int)res];
    }
}