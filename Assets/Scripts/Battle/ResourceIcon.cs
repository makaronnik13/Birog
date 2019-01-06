using UnityEngine;
using UnityEngine.UI;

public class ResourceIcon : MonoBehaviour
{
    public Image ResourceImage;

    private CardStats.Resources _res;

    public CardStats.Resources Res
    {
        get
        {
            return _res;
        }
    }

    public void Init(CardStats.Resources res, Transform parent)
    {
        _res = res;
        ResourceImage.sprite = DefaultResources.GetResourceSprite(res);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void Init(CardStats.CardType cardType, Transform parent)
    {
        ResourceImage.sprite = DefaultResources.GetNeedCardSprite(cardType);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
}