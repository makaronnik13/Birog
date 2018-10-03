using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterDeck : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponentInChildren<CardCollider>().OnClicked += DeckClicked;
	}

    private void DeckClicked()
    {
           
    }
 
}
