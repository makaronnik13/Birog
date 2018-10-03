using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour {

    public TextMeshProUGUI Title, Description;
    public Image CardImg;


private ICard _card;

	public void Init(ICard card)
    {
        _card = card;
        Title.text = card.CardName();
        Description.text = card.CardDescription();
        CardImg.sprite = card.CardImage();

    }

    /*
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in PhotonPlayer.TagObject
        BattleCard c = DefaultResources.GetCardById((int)photonView.InstantiationData[0]);
        bool show = photonView.Owner == PhotonNetwork.LocalPlayer;
        CardGameManager.CardPosition position = (CardGameManager.CardPosition)((int)photonView.InstantiationData[1]);

        Init(c, position, show);
    }
    */
}
