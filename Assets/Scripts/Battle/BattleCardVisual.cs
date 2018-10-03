using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Photon.Pun;

public class BattleCardVisual : CardVisual
{
    private int _lastSibling;
   
    private bool _hovered = false;
    private Transform _oldParentTransform;

    public TextMeshProUGUI CardType;
    public RectTransform ResourcesSlot;


    public void Init(BattleCard card, bool show)
    {
        base.Init(card);

        CardType.text = card.CardType.ToString();

        foreach (KeyValuePair<CardStats.Resources, int> val in card.Resources)
        {
            for (int i = 0; i< val.Value;i++)
            {
                GameObject resourceIcon = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.ResourceIcon));
                resourceIcon.GetComponent<ResourceIcon>().Init(val.Key,ResourcesSlot);          
            }
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardGameManager.Instance.CardClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CardGameManager.Instance.CanFocus(this))
        {
            return;
        }
        _hovered = true;

        //Reposition(GetComponentInParent<CardsLayout>());

        _lastSibling = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_lastSibling!=-1)
        {
            transform.SetSiblingIndex(_lastSibling);
        }
        
        _hovered = false;
        //Reposition(GetComponentInParent<CardsLayout>());
    }

    




}
