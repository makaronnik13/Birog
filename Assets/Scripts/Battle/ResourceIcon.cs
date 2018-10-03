using UnityEngine;
using UnityEngine.UI;

public class ResourceIcon : MonoBehaviour
{
    public Image ResourceImage;

    public void Init(CardStats.Resources res, Transform parent)
    {
        ResourceImage.sprite = DefaultResources.GetResourceSprite(res);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
}