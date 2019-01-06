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
            ServerController.Instance.photonView.RPC("PlayerClickedOnDeckRPC", RpcTarget.MasterClient, new object[] { PhotonNetwork.LocalPlayer.ActorNumber});
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



    //recieve

    [PunRPC]
    private void DeckClicked(int cardId, string cardGuid)
    {
        EncounterCardWrapper wrapper = new EncounterCardWrapper((EncounterCard)DefaultResources.GetCardById(cardId, DefaultResources.CardType.Encounter), cardGuid);

        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.CreateCardIn(wrapper, CardsLayoutManager.SlotType.EncounterDeck);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.CurrentEncounter, ()=> 
        {
            cardBehaviour.GetComponent<EncounterCardVisual>().ShowVariants();
        });
    }

    [PunRPC]
    private void GetEventCard(int cardId, string guid)
    {
        EventCardWrapper wrapper = new EventCardWrapper((EventCard)DefaultResources.GetCardById(cardId, DefaultResources.CardType.Event), guid); 
        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.CreateCardIn(wrapper, CardsLayoutManager.SlotType.EventDeck);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.EventDrop);
    }

    [PunRPC]
    private void TakeCardFromDeck(int playerId, int cardId, string cardGuid)
    {
        Debug.Log("TC1");

        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber) //visualise giving cards only for local player
        { 
            TakeCard((BattleCard)DefaultResources.GetCardById(cardId, DefaultResources.CardType.Battle), cardGuid);
        }
    }


    [PunRPC]
    private void GiveCardToPlayer(int playerId, int cardId, string cardGuid)
    {
        if (playerId == PhotonNetwork.LocalPlayer.ActorNumber) //visualise giving cards only for local player
        {
            AddCardToDeck((BattleCard)DefaultResources.GetCardById(cardId, DefaultResources.CardType.Battle), cardGuid);
            //TakeCard((BattleCard)DefaultResources.GetCardById(cardId));
        }
    }

    [PunRPC]
    private void HideEncounterCard()
    {
        CardsLayoutManager.Instance.HideEncounterCard();
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

    private void TakeCard(BattleCard card, string guid)
    {
        Debug.Log("TC2");
        CardBehaviour cardBehaviour = AddCardToDeck(card, guid);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.PlayerHand);
    }

    private CardBehaviour AddCardToDeck(BattleCard card, string guid)
    {
        BattleCardWrapper wrapper = new BattleCardWrapper(card, guid);
        CardBehaviour cardBehaviour = CardsLayoutManager.Instance.CreateCardIn(wrapper, CardsLayoutManager.SlotType.Nowhere);
        CardsLayoutManager.Instance.MoveCardTo(cardBehaviour, CardsLayoutManager.SlotType.PlayerDeck, null, true);
        return cardBehaviour;
    }
}
