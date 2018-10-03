﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CardsLayout : MonoBehaviour
{
    public bool StaticSlot = false;
    public Vector3 FocusDelta;
	private List<Transform> CardsSiblings = new List<Transform>();
    private Vector2 _cardSize = Vector2.zero;
    private Vector2 cardSize
    {
        get
        {
            if (_cardSize == Vector2.zero)
            {
                _cardSize = FindObjectOfType<CardBehaviour>().GetComponent<RectTransform>().rect.size;
            }
            return _cardSize;
        }
    }

	public List<CardBehaviour> Cards
	{
		get
		{
			List<CardBehaviour> cv = new List<CardBehaviour> ();
			foreach(Transform pair in CardsSiblings)
			{
					cv.Add (pair.GetComponent<CardBehaviour>());
			}
				
			return cv;
		}
	}
    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    public float rotOffset = 3;
    public float maxRot = 20;
	public float gap = 0;

    public Action<CardBehaviour> OnCardAddedToLayout = (cv) => { };
    public Action<CardBehaviour> OnCardRemovedFromLayout = (cv) => { };

    public int GetCardSibling(CardBehaviour cv)
	{
		return CardsSiblings.IndexOf(cv.transform);
	}
	public void AddCardToLayout(CardBehaviour visual)
	{
        if (CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetSiblingIndex(CardsSiblings.IndexOf(visual.transform));
        }
		else
		{
            visual.transform.SetParent(transform);
            
            OnCardAddedToLayout.Invoke(visual);
            CardsSiblings.Add (visual.transform);
        }
        CardsReposition();
    }
	public void RemoveCardFromLayout(CardBehaviour visual)
	{
       
        if (CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetParent(null);
            OnCardRemovedFromLayout(visual);
            CardsSiblings.Remove(visual.transform);
        }
        CardsReposition();
    }

    [ContextMenu("Reposition")]
	public void CardsReposition()
	{
		foreach(Transform pair in CardsSiblings)
		{
            pair.GetComponent<CardBehaviour>().Reposition(this);
		}
	}

    public Quaternion GetRotation(CardBehaviour cardVisual, bool focused = false)
    {
        if (StaticSlot)
        {
            return Quaternion.identity;
        }

        int cards = transform.childCount;
        Quaternion aimRotation = Quaternion.identity;
        if (rotOffset != 0)
        {
            float offset = Mathf.Min(rotOffset, maxRot / cards);
            int childId = CardsSiblings.IndexOf(cardVisual.transform);
            float minOffset = -(cards - 1) * offset / 2;
            float rot = (minOffset + childId * offset);

            if (!focused)
            {
                aimRotation = Quaternion.Euler(new Vector3(0, 0, -rot));
            }
        }
        return aimRotation;
    }
    public Vector3 GetPosition(CardBehaviour cardVisual, bool focused = false)
    {

        if (StaticSlot)
        {
            if (GetComponent<GridLayout>())
            {
                return cardVisual.transform.localPosition;
            }
            return Vector3.zero;
        }

        float yMultiplyer = 1f / 10000;
        int cards = transform.childCount;
        float fieldWidth = GetComponent<RectTransform>().rect.width;
		float cardWidth = cardSize.x;
		float offset = Mathf.Min(cardWidth, fieldWidth/cards)+gap;

        Vector3 aimPosition = Vector3.zero;
        int childId = CardsSiblings.IndexOf(cardVisual.transform);
        
        float minOffset = -(cards - 1) * offset / 2;

        float yPos = -Mathf.Pow(minOffset + childId * offset, 2) * yMultiplyer;
        aimPosition = new Vector3(minOffset+childId*offset, yPos);



        if (focused)
        {
            aimPosition += FocusDelta;
        }

        aimPosition = new Vector3(aimPosition.x, aimPosition.y, aimPosition.z);

        return aimPosition;
    }
}