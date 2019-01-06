using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Linq;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerData
{
    public int Hp
    {
        get
        {
            object result;
            player.CustomProperties.TryGetValue(DefaultResources.PLAYER_HP, out result);
            return (int)result;
        }
        set
        {
            Hashtable props = new Hashtable
            {
                {DefaultResources.PLAYER_HP, value}
            };
            player.SetCustomProperties(props);
        }
    }
    public int Armor
    {
        get
        {
            object result;
            player.CustomProperties.TryGetValue(DefaultResources.PLAYER_ARMOR, out result);
            return (int)result;
        }
        set
        {
            Hashtable props = new Hashtable
            {
                {DefaultResources.PLAYER_ARMOR, value}
            };
            player.SetCustomProperties(props);
        }
    }
    public int Initiative
    {
        get
        {
            object result;
            player.CustomProperties.TryGetValue(DefaultResources.PLAYER_INITIATIVE, out result);
            return (int)result;
        }
        set
        {
            Hashtable props = new Hashtable
            {
                {DefaultResources.PLAYER_INITIATIVE, value}
            };
            player.SetCustomProperties(props);
        }
    }

    public Player player;
    public Queue<BattleCardWrapper> Deck = new Queue<BattleCardWrapper>();
    public List<BattleCardWrapper> Hand = new List<BattleCardWrapper>();
    public List<BattleCardWrapper> Drop = new List<BattleCardWrapper>();

    public PlayerData(Player player, List<BattleCard> cards, int hp, int armor, int initiative)
    {
        this.player = player;
        List<BattleCardWrapper> wrappers = new List<BattleCardWrapper>();
        foreach (BattleCard bc in cards)
        {
            wrappers.Add(new BattleCardWrapper(bc));
        }

        this.Deck = new Queue<BattleCardWrapper>(wrappers.OrderBy(c=>Guid.NewGuid()));

        Hp = hp;
        Initiative = initiative;
        Armor = armor;
    }

    public void UpdateDeck()
    {
        Debug.Log("Reshuffle");
        Deck = new Queue<BattleCardWrapper>(Drop.OrderBy(c=>Guid.NewGuid()));
    }
}
