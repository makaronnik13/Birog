using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerController : MonoBehaviourPun
{

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


    [PunRPC]
    private void PlayerClickedOnDeckRPC(int viewId, string playerName)
    {
        EncounterDeck deck = PhotonNetwork.GetPhotonView(viewId).GetComponent<EncounterDeck>();
        Player player = PhotonNetwork.PlayerList.ToList().FirstOrDefault(p => p.NickName == playerName);

        if (GameStateMachine.Instance.CanPlayerPickCardFromEncounterDeck(deck, player))
        {
            EcnounterCard card = BoardData.Instance.GetNextCard(deck);
            ClientController.Instance.photonView.RPC("DeckClicked", RpcTarget.All, new object[] {DefaultResources.GetCardId(card)});
        }
    }
}
