using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DefaultResources
{
    //player custom properties keys
    public static string PLAYER_LOADED_LEVEL = "LevelLoaded";
    public static string PLAYER_CLASS = "PlayerClass";
    public static string PLAYER_IS_READY = "PlayerIsReady";
    public static string IS_ACTIVE_PLAYER = "ActivePlayer";
    public static string PLAYER_HP = "PlayerHp";
    public static string PLAYER_ARMOR = "PlayerArmor";
    public static string PLAYER_INITIATIVE = "PlayerInitiative";

    public static int CardsOnHand = 5;

    public enum CardType
    {
        Battle,
        Encounter,
        Event
    }

    private static Dictionary<CardStats.CardType, Color> _typesColors = new Dictionary<CardStats.CardType, Color>()
    {
        {CardStats.CardType.Minion, new Color(0.3f, 0.7f, 0.3f, 1f)},
        {CardStats.CardType.Mutation, new Color(0.5f, 0.2f, 0.5f, 1f)},
        {CardStats.CardType.Weapon, new Color(0.7f, 0.3f, 0.3f, 1f)},
        {CardStats.CardType.Fint, new Color(0.3f, 0.3f, 0.7f, 1f)}
    };

    public static Color GetCardColor(CardStats.CardType cardType)
    {
        return _typesColors[cardType];
    }

    public static string PLAYER_DROP = "PlayerDrop";

    //events codes
    public static byte START_GAME_EVENT = 0;

    private static EncounterCard[] _encounterCards = null;

    public static int CardsForInitiative(int initiative)
    {
        int cards = 1;


        if (UnityEngine.Random.value<initiative/25f)
        {
            cards++;
        }

        if (UnityEngine.Random.value < Mathf.Clamp(initiative-10,0,initiative) / 25f)
        {
            cards++;
        }

        if (UnityEngine.Random.value < Mathf.Clamp(initiative - 20, 0, initiative) / 15f)
        {
            cards++;
        }

        Debug.Log(cards);

        return cards;
    }

    public static EncounterCard[] EncounterCards
    {
        get
        {
            if (_encounterCards == null)
            {
                _encounterCards = Resources.LoadAll<EncounterCard>("Cards");
            }
            return _encounterCards;
        }
    }

    private static BattleCard[] _battleCards = null;
    public static BattleCard[] BattleCards
    {
        get
        {
            if (_battleCards == null)
            {
                _battleCards = Resources.LoadAll<BattleCard>("Cards");
            }
            return _battleCards;
        }
    }

    private static EventCard[] _eventCards = null;
    public static EventCard[] EventCards
    {
        get
        {
            if (_eventCards == null)
            {
                _eventCards = Resources.LoadAll<EventCard>("Cards");
            }
            return _eventCards;
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


    public static ICard GetCardById(int i, CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Battle:
                return BattleCards.FirstOrDefault(c => c.Id == i);
            case CardType.Encounter:
                return EncounterCards.FirstOrDefault(c => c.Id == i);
            case CardType.Event:
                return EventCards.FirstOrDefault(c => c.Id == i);
        }

        return null;
    }


    public enum PrefabType
    {
        BattleCard,
        CardNeed,
        Deck,
        EncounterCard,
        EventCard,
        PlayerListEntry,
        ResourceIcon,
        VariantButton
    }

    private static List<GameObject> _prefabs = new List<GameObject>();
    private static List<GameObject> prefabs
    {
        get
        {
            if (_prefabs.Count == 0)
            {
                _prefabs = Resources.LoadAll<GameObject>("Prefabs").ToList();
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

    private static Sprite[] _cardsSprites = null;
    private static Sprite[] cardsSprites
    {
        get
        {
            if (_cardsSprites == null)
            {
                _cardsSprites = Resources.LoadAll<Sprite>("Sprites/CardsIcons");
            }
            return _cardsSprites;
        }
    }

    public static Sprite GetNeedCardSprite(CardStats.CardType cardType)
    {
        return cardsSprites[(int)cardType];
    }

}