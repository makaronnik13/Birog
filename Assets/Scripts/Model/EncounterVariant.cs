using Malee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class EncounterVariant
{
    [HideLabel]
    [MultiLineProperty(2)]
    public string Description;


    [InlineProperty]
    [Title("Cost", TitleAlignment = TitleAlignments.Left, Bold = true, HorizontalLine = false)]
    [HideLabel]
    public Cost VariantCost;

    [Title("Effects", TitleAlignment = TitleAlignments.Left, Bold = true, HorizontalLine = false)]
    public List<EncounterEffect> Effects;
}
