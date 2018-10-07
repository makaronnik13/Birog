using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class CardsLayout : MonoBehaviour
{
    public bool StaticSlot = false;
    public bool Fit = false;

    [Range(0,1f)]
    public float FocusDelta;
    public Vector3 FocusOffset;

    private Dictionary<CardBehaviour, Action> callbacks = new Dictionary<CardBehaviour, Action>();

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

    private bool _shoundReposition = false;

    public Action<CardBehaviour> OnCardAddedToLayout = (cv) => { };
    public Action<CardBehaviour> OnCardRemovedFromLayout = (cv) => { };

    private void Start()
    {
        InvokeRepeating("CardsReposition", 0, 0.1f);
    }

    public int GetCardSibling(CardBehaviour cv)
	{
		return CardsSiblings.IndexOf(cv.transform);
	}
	public void AddCardToLayout(CardBehaviour visual, Action callback = null)
	{
        if (callback!=null)
        {
            callbacks.Add(visual, callback);
        }
        

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

        _shoundReposition = true;
    }
	public void RemoveCardFromLayout(CardBehaviour visual)
	{     
        if (CardsSiblings.Contains(visual.transform))
        {
            visual.transform.SetParent(null);
            OnCardRemovedFromLayout(visual);
            CardsSiblings.Remove(visual.transform);
        }
        _shoundReposition = true;
    }

	public void CardsReposition()
	{
        if (_shoundReposition)
        {
            foreach (Transform pair in CardsSiblings)
            {
                Action callback = null;
                CardBehaviour cardBehaviour = pair.GetComponent<CardBehaviour>();
                if (callbacks.ContainsKey(cardBehaviour))
                {
                    callback = callbacks[cardBehaviour];
                    callbacks.Remove(cardBehaviour);
                }
                cardBehaviour.Reposition(this, callback);
            }
            _shoundReposition = false;
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
                aimRotation = Quaternion.Euler(new Vector3(0, rot, 0));
            }
        }
        return aimRotation;
    }
    public Vector3 GetPosition(CardBehaviour cardVisual)
    {
        int cards = transform.childCount;
   
		float cardWidth = cardSize.x+gap;

        if (Fit)
        {
            cardWidth = Mathf.Min(cardWidth, GetComponent<RectTransform>().rect.width/cards);
        }

		float offset = cardWidth;

        Vector3 aimPosition = Vector3.zero;
        int childId = CardsSiblings.IndexOf(cardVisual.transform);
        
        float minOffset = -(cards - 1) * offset / 2;

        float yPos = 0;
        aimPosition = new Vector3(minOffset+childId*offset, childId * cardSize.y, yPos);

        if (StaticSlot)
        {
            aimPosition = Vector3.zero;
        }

        if (cardVisual.Focused)
        {
            aimPosition = Vector3.Lerp(aimPosition, transform.InverseTransformPoint(Camera.main.transform.position), FocusDelta)+FocusOffset;
        }


        return aimPosition;
    }

    [ContextMenu("Test")]
    private void TestLayout()
    {
        _shoundReposition = true;
        foreach (CardBehaviour cb in FindObjectsOfType<CardBehaviour>())
        {
            AddCardToLayout(cb);
        }   
    }
}
