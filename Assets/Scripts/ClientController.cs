using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ClientController : MonoBehaviourPunCallbacks
{
    private static ClientController _instance;
    public static ClientController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ClientController>();
            }
            return _instance;
        }
    }

    //send
    public void PlayerClickedOnDeck(EncounterDeckCollider deck)
    {
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties[DefaultResources.IS_ACTIVE_PLAYER])
        {
            ServerController.Instance.photonView.RPC("PlayerClickedOnDeckRPC", RpcTarget.MasterClient, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, deck.GetComponent<PhotonView>().ViewID });
        }
    }

    public void PlayCard()
    {
        ServerController.Instance.photonView.RPC("PlayerPlayedCardRPC", RpcTarget.MasterClient, new object[] {PhotonNetwork.LocalPlayer.ActorNumber});
    }

    public void MakeChoice(int variantId)
    {
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties[DefaultResources.IS_ACTIVE_PLAYER])
        {
            ServerController.Instance.photonView.RPC("PlayerMakeChoiceRPC", RpcTarget.MasterClient, new object[] { variantId });
        }
    }

    public void PayCost(int variantId)
    {
        ServerController.Instance.photonView.RPC("PlayerPayCostRPC", RpcTarget.MasterClient, new object[] { PhotonNetwork.LocalPlayer.ActorNumber, variantId });
    }

    public void EndTurn()
    {
        ServerController.Instance.photonView.RPC("PlayerEndTurnRPC", RpcTarget.MasterClient, new object[] { });
    }


    //recieve

    [PunRPC]
    private void DeckClicked(int cardId, int deckId)
    {
        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.CreateCardIn((EncounterCard)DefaultResources.GetCardById(cardId), CardsLayoutManager.SlotType.EncounterDeck, deckId);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.CurrentEncounter, -1, ()=> {
            cardBehaviour.GetComponent<EncounterCardVisual>().ShowVariants();
        });
    }

    [PunRPC]
    private void GetEventCard(int cardId)
    {
        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.CreateCardIn((EventCard)DefaultResources.GetCardById(cardId), CardsLayoutManager.SlotType.EventDeck);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.EventDrop);
    }

    [PunRPC]
    private void TakeCardFromDeck(int playerId, int cardId)
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber) //visualise giving cards only for local player
        { 
            TakeCard((BattleCard)DefaultResources.GetCardById(cardId));
        }
    }


    [PunRPC]
    private void GiveCardToPlayer(int playerId, int cardId)
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber) //visualise giving cards only for local player
        {
            AddCardToDeck((BattleCard)DefaultResources.GetCardById(cardId));
            //TakeCard((BattleCard)DefaultResources.GetCardById(cardId));
        }
    }

    [PunRPC]
    private void HideEncounterCard(int deckId)
    {
        CardsLayoutManager.Instance.HideEncounterCard(deckId);
    }

    private void Start()
    {
        Hashtable props = new Hashtable() { { DefaultResources.PLAYER_LOADED_LEVEL, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(DefaultResources.IS_ACTIVE_PLAYER))
        {
            if (targetPlayer == PhotonNetwork.LocalPlayer)
            {
                
            }
        }
    }

    private void TakeCard(BattleCard card)
    {
        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.GetCardFrom(card, CardsLayoutManager.SlotType.PlayerDeck);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.PlayerHand);
    }

    private void AddCardToDeck(BattleCard card)
    {
        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.CreateCardIn(card, CardsLayoutManager.SlotType.Nowhere);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.PlayerDeck);
    }
}
