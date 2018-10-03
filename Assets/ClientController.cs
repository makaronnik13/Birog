using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class ClientController : MonoBehaviourPun
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

    public void PlayerClickedOnDeck(EncounterDeck deck)
    {
        ServerController.Instance.photonView.RPC("PlayerClickedOnDeckRPC", RpcTarget.MasterClient, new object[] {deck.GetComponent<PhotonView>().ViewID, PhotonNetwork.LocalPlayer.NickName});
    }
}
