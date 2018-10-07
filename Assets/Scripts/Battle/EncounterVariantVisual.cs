using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class EncounterVariantVisual : MonoBehaviour
{
    public TextMeshProUGUI Description;

    private Action _onClick;

    public void Init(EncounterVariant variant, Action onClick)
    {
        _onClick = onClick;
        GetComponent<Button>().onClick.AddListener(OnClick);
        Description.text = variant.Description;
    }

    private void OnClick()
    {
        _onClick();
    }
}