using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class EncounterVariantVisual : MonoBehaviour
{
    public TextMeshProUGUI Description;

    public Transform NeedsTransform;

    private Action _onClick;


    public void Init(EncounterVariant variant, Action onClick)
    {
        _onClick = onClick;
      
        Description.text = variant.Description;
        foreach (CardStats.Resources res in variant.VariantCost.Resources)
        {
            GameObject needIcon = DefaultResources.GetPrefab(DefaultResources.PrefabType.CardNeed);
            needIcon.transform.SetParent(NeedsTransform);
            needIcon.transform.localPosition = Vector3.zero;
            needIcon.transform.localScale = Vector3.one;
            needIcon.GetComponent<NeedIcon>().Init(res);
        }
    }

    public void SetNeeds(List<BattleCard> cards)
    {
        List<CardStats.Resources> res = new List<CardStats.Resources>();
        foreach (BattleCard bc in cards)
        {
            foreach (ResourcePair pair in bc.Resources)
            {
                for (int i = 0; i<pair.Value;i++)
                {
                    res.Add(pair.Resource);
                }
            }
        }

        foreach (NeedIcon ni in NeedsTransform.GetComponentsInChildren<NeedIcon>())
        {
            if (res.Contains(ni.ResIcon.Res))
            {
                res.Remove(ni.ResIcon.Res);
                ni.Payed = true;
            }
            else
            {
                ni.Payed = false; 
            }
            
        }
    }

}