using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleCardVisual : CardVisual
{
    public RectTransform ResourcesSlot;
    public Image[] ColoringPanels;

    public void Init(BattleCardWrapper card, bool show)
    {
        base.Init(card.Card, card.Guid);

        foreach (ResourcePair val in card.Card.Resources)
        {
            for (int i = 0; i < val.Value; i++)
            {
                GameObject resourceIcon = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.ResourceIcon));
                resourceIcon.GetComponent<ResourceIcon>().Init(val.Resource, ResourcesSlot);
                resourceIcon.transform.localRotation = Quaternion.identity;
            }
        }

        foreach (Image panel in ColoringPanels)
        {
            panel.color = DefaultResources.GetCardColor(((BattleCard)card.Card).CardType);
        }
    }
}
