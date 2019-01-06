using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class EncounterCardVisual : CardVisual
{
    public Transform VariantsSlot;

    private Animator __variantsAnimator;
    private Animator _variantsAnimator
    {
        get
        {
            if (!__variantsAnimator)
            {
                __variantsAnimator = GetComponent<Animator>();
            }
            return __variantsAnimator;
        }
    }

    private Action _hideCallback;

    public void Init(EncounterCardWrapper card, bool show)
    {
        base.Init(card.Card, card.Guid);
    }

    public void ShowVariants()
    {
        Debug.Log("OPENN!!!");
        _variantsAnimator.SetBool("Open", true);
    }

    public void HideVariants(Action callback)
    {
        _hideCallback = callback;
        foreach (Transform t in VariantsSlot)
        {
            Destroy(t.gameObject);
        }
        _variantsAnimator.SetBool("Open", false);
    }

    private void VariantChoosed(int i)
    {
        ClientController.Instance.MakeChoice(i);
    }

    public void OnOpened()
    {
        int i = 0;
        foreach (EncounterVariant variant in ((EncounterCard)_card).Variants)
        {
            GameObject variantGo = Instantiate(DefaultResources.GetPrefab(DefaultResources.PrefabType.VariantButton));
            variantGo.transform.SetParent(VariantsSlot);
            variantGo.transform.localScale = Vector3.one;
            variantGo.transform.localPosition = Vector3.zero;
            variantGo.transform.localRotation = Quaternion.identity;
            variantGo.GetComponent<EncounterVariantVisual>().Init(variant, () => { VariantChoosed(i); });
            i++;
        }
    }

    public void OnCLosed()
    {
        _hideCallback();
    }
}