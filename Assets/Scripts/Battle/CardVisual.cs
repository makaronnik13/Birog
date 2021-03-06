﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour {

    public TextMeshProUGUI Title, Description;
    public Image CardImg;
    public string Guid;

    public ICard _card;

	public void Init(ICard card, string guid)
    {
        Guid = guid;
        _card = card;
        Title.text = card.CardName();
        Description.text = card.CardDescription();
        CardImg.sprite = card.CardImage();
    }
}
