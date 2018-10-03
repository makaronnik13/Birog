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

    
    public RectTransform ResourcesSlot;
    public Image[] ColoringPanels;

    public void Init(BattleCard card, bool show)
    {
        base.Init(card);

        Debug.Log(card.Resources.Count);

        foreach (ResourcePair val in card.Resources)
        {
            Debug.Log(val.Value);

            for (int i = 0; i < val.Value; i++)
            {
                GameObject resourceIcon = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.ResourceIcon));
                resourceIcon.GetComponent<ResourceIcon>().Init(val.Resource, ResourcesSlot);
                resourceIcon.transform.localRotation = Quaternion.identity;
            }

        }

        foreach (Image panel in ColoringPanels)
        {
            panel.color = DefaultResources.GetCardColor(((BattleCard)card).CardType);
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
        if (_lastSibling != -1)
        {
            transform.SetSiblingIndex(_lastSibling);
        }

        _hovered = false;
        //Reposition(GetComponentInParent<CardsLayout>());
    }






}
