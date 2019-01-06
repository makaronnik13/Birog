using Malee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/EncounterCard", order = 1)]
public class EncounterCard : ScriptableObject, ICard
{
    public int Id;

    [HideLabel]
    public string CardName;
    [HideLabel]
    [MultiLineProperty(6)]
    [HorizontalGroup("G1")]
    public string Description;

    [HorizontalGroup("G1", Width = 100)]
    [HideLabel]
    [PreviewField(Height = 100)]//(InlineEditorModes.LargePreview, DrawGUI = false, DrawHeader = false, ObjectFieldMode = InlineEditorObjectFieldModes.Boxed, Expanded = false)]
    public Sprite Image;

    public List<EncounterVariant> Variants = new List<EncounterVariant>();

    public string CardDescription()
    {
        return Description;
    }

    public Sprite CardImage()
    {
        return Image;
    }

    string ICard.CardName()
    {
        return CardName;
    }

}
