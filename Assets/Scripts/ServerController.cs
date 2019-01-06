using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ServerController : MonoBehaviourPunCallbacks
{
    public Quest FakeQuest;
    private static ServerController _instance;
    public static ServerController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ServerController>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        GameStateMachine.Instance.OnPlayerTurnStarted += StartPlayerTurn;
        GameStateMachine.Instance.OnRoundStarted += RoundStarted;
    }

    private void RoundStarted()
    {
        EventCardWrapper card = BoardData.Instance.GetNextEventCard();
        ClientController.Instance.photonView.RPC("GetEventCard", RpcTarget.All, new object[] { card.CardId, card.Guid});
    }

    private void StartPlayerTurn(Player player)
    {
        Debug.Log("Start turn");

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Hashtable props = new Hashtable() { { DefaultResources.IS_ACTIVE_PLAYER, p == player } };
            p.SetCustomProperties(props);
        }

        foreach (BattleCardWrapper bc in BoardData.Instance.TakeCards(player, DefaultResources.CardsForInitiative(BoardData.Instance.GetPlayer(player).Initiative)))
        {
            TakeCardFromDeck(player, bc);
        }     
    }

    [PunRPC]
    private void PlayerClickedOnDeckRPC(int playerId)
    {
        if (GameStateMachine.Instance.CanClickEncounterDeck)
        {
            EncounterCardWrapper card = BoardData.Instance.GetNextEncounterCard();
            BoardData.Instance.CurrentEncounter = card.CardId;
            ClientController.Instance.photonView.RPC("DeckClicked", RpcTarget.All, new object[] { card.CardId, card.Guid });
            GameStateMachine.Instance.CanClickEncounterDeck = false;
        }
    }

    [PunRPC]
    private void PlayerPlayedCardRPC(int playerId)
    {

    }

    [PunRPC]
    private void PlayerMakeChoiceRPC(int variantId)
    {
        Debug.Log("Choose variant "+variantId+" from card: "+ BoardData.Instance.CurrentEncounter);
        ClientController.Instance.photonView.RPC("HideEncounterCard", RpcTarget.All, new object[] {});
        GameStateMachine.Instance.EndPlayerTurn();
    }

    [PunRPC]
    private void PlayerPayCostRPC(int playerId, int variantId)
    {

    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (changedProps.ContainsKey(DefaultResources.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                StartGame();
            }
        }

    }

    private void StartGame()
    {
        Debug.Log("Start game");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            BattlerClass battlerClass = DefaultResources.GetClassById((int)player.CustomProperties[DefaultResources.PLAYER_CLASS]);
            BoardData.Instance.AddPlayer(player, battlerClass.Deck.ToList(), battlerClass.Hp, battlerClass.Armor, battlerClass.Initiative);

            foreach (BattleCardWrapper bc in BoardData.Instance.TakeCards(player, DefaultResources.CardsOnHand))
            {
                 TakeCardFromDeck(player, bc);
            }
        }

        List<Player> playersQueque = PhotonNetwork.PlayerList.OrderBy(g => Guid.NewGuid()).ToList();

        BoardData.Instance.InitBoardData(FakeQuest.EncounterDeck.cards, FakeQuest.EventsDeck.Cards);

        GameStateMachine.Instance.StartGame(playersQueque);
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties[DefaultResources.PLAYER_LOADED_LEVEL])
            {
                return false;
            }
        }

        return true;
    }

    private void TakeCardFromDeck(Player player, BattleCardWrapper cardWrapper)
    {
        Debug.Log("Take from deck");
        ClientController.Instance.photonView.RPC("TakeCardFromDeck", RpcTarget.All, new object[] { player.ActorNumber, cardWrapper.CardId, cardWrapper.Guid});
    }
}
