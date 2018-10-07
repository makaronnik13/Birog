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
        EventCard card = BoardData.Instance.GetNextEventCard();
        ClientController.Instance.photonView.RPC("GetEventCard", RpcTarget.All, new object[] { DefaultResources.GetCardId(card)});
    }

    private void StartPlayerTurn(int playerId)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Hashtable props = new Hashtable() { { DefaultResources.IS_ACTIVE_PLAYER, p.ActorNumber == playerId } };
            p.SetCustomProperties(props);
        }
    }

    [PunRPC]
    private void PlayerClickedOnDeckRPC(int playerId, int viewId)
    {
        EncounterDeckCollider deck = PhotonNetwork.GetPhotonView(viewId).GetComponent<EncounterDeckCollider>();
        int deckId = FindObjectsOfType<EncounterDeckCollider>().ToList().IndexOf(deck);
        BoardData.Instance.LastEncounterDeckId = deckId+15;
        EncounterCard card = BoardData.Instance.GetNextEncounterCard(deckId);
        BoardData.Instance.CurrentEncounter = DefaultResources.GetCardId(card);
        ClientController.Instance.photonView.RPC("DeckClicked", RpcTarget.All, new object[] {DefaultResources.GetCardId(card), viewId});   
    }

    [PunRPC]
    private void PlayerPlayedCardRPC(int playerId)
    {

    }

    [PunRPC]
    private void PlayerMakeChoiceRPC(int variantId)
    {
        Debug.Log("Choose variant "+variantId+" from card: "+ BoardData.Instance.CurrentEncounter);

        ClientController.Instance.photonView.RPC("HideEncounterCard", RpcTarget.All, new object[] { BoardData.Instance.LastEncounterDeckId});
    }

    [PunRPC]
    private void PlayerPayCostRPC(int playerId, int variantId)
    {

    }

    [PunRPC]
    private void PlayerEndTurnRPC()
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
        Dictionary<int, List<BattleCard>> playerCards = new Dictionary<int, List<BattleCard>>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            foreach (BattleCard bc in DefaultResources.GetClassById((int)player.CustomProperties[DefaultResources.PLAYER_CLASS]).Deck)
            {
                GiveCardToPlayer(player, DefaultResources.GetCardId(bc));
            }
            List<BattleCard> cards = DefaultResources.GetClassById((int)player.CustomProperties[DefaultResources.PLAYER_CLASS]).Deck.ToList();  //get fake card-list from player's class
            playerCards.Add(player.ActorNumber, cards);
        }

        List<int> playersQueque = PhotonNetwork.PlayerList.Select(p => p.ActorNumber).OrderBy(g => Guid.NewGuid()).ToList();

        BoardData.Instance.InitBoardData(FakeQuest.EncounterDecks, FakeQuest.EventsDeck, playerCards);

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

    private void GiveCardToPlayer(Player player, int cardId)
    {
        ClientController.Instance.photonView.RPC("GiveCardToPlayer", RpcTarget.All, new object[] { player.ActorNumber, cardId});
    }
}
