using SpellGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerDeck : MonoBehaviour {

    public CardsLayout PreviewLayout;

	// Use this for initialization
	void Start ()
    {
        GetComponentInChildren<CardCollider>().OnClicked += DeckClicked;
	}

    private void DeckClicked()
    {
        /*
        foreach (BattleCard bc in DefaultResources.GetClassById(Player.Instance.PlayerClass).Deck)
        {
            GameObject card = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.BattleCard));

            card.transform.localScale = Vector3.one;

            card.transform.SetParent(transform);
            
            card.transform.position = transform.position;         
            card.transform.localRotation = Quaternion.identity;
            card.GetComponent<BattleCardVisual>().Init(bc, true);
            PreviewLayout.AddCardToLayout(card.GetComponent<CardBehaviour>());
        }
        */
    }

    
}
