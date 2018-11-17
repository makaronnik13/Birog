using Sirenix.OdinInspector;

[System.Serializable]
public class ResourcePair
{
    [HideLabel]
    [HorizontalGroup("Group 1"), LabelWidth(0)]
    public CardStats.Resources Resource;

    [HideLabel]
    [HorizontalGroup("Group 1"), LabelWidth(0)]
    public int Value;
}