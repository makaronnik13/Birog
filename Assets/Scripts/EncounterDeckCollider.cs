using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterDeckCollider : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponentInChildren<CardCollider>().OnClicked += DeckClicked;
	}

    private void DeckClicked()
    {
        ClientController.Instance.PlayerClickedOnDeck(this);
    }
 
}
