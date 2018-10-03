using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class CardsLayout : MonoBehaviour
{
    public bool StaticSlot = false;
    [Range(0,1f)]
    public float FocusDelta;
	private List<Transform> CardsSiblings = new List<Transform>();
    private Vector2 _cardSize = Vector2.zero;
    private Vector2 cardSize
    {
        get
        {
            if (_cardSize == Vector2.zero)
            {
                _cardSize = GetComponentInChildren<CardBehaviour>().Size;
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
    public Vector3 GetPosition(CardBehaviour cardVisual)
    {

        if (StaticSlot)
        {
            return Vector3.zero;
        }


        int cards = transform.childCount;
   
		float cardWidth = cardSize.x+gap;
		float offset = cardWidth;

        Vector3 aimPosition = Vector3.zero;
        int childId = CardsSiblings.IndexOf(cardVisual.transform);
        
        float minOffset = -(cards - 1) * offset / 2;

        float yPos = 0;
        aimPosition = new Vector3(minOffset+childId*offset, yPos);

        if (cardVisual.Focused)
        {
            aimPosition = Vector3.Lerp(aimPosition, transform.InverseTransformPoint(Camera.main.transform.position), FocusDelta);
        }


        return aimPosition;
    }
}
