using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsLayoutManager : Singleton<CardsLayoutManager> {

    public CardsLayout CardInfoSlot, PlayerHand, Deck;

    private CardBehaviour _focusedCard;
	public CardBehaviour FocusedCard
    {
        get
        {
            return _focusedCard;
        }
        set
        {
            if (_focusedCard == value)
            {
                return;
            }

            if (_focusedCard)
            {
                Deck.AddCardToLayout(_focusedCard);
            }

            _focusedCard = value;

            if (FocusedCard)
            {
                CardInfoSlot.AddCardToLayout(_focusedCard);
            }
            
        }
    }

    private void Start()
    {
        foreach (CardBehaviour cb in FindObjectsOfType<CardBehaviour>())
        {
            Deck.AddCardToLayout(cb);
        }
    }

}
