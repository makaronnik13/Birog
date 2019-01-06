using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedIcon : MonoBehaviour {

    public ResourceIcon ResIcon;

    private bool _payed = false;
    public bool Payed
    {
        get
        {
            return _payed;
        }
        set
        {
            _payed = value;

            if (_payed)
            {
                ResIcon.GetComponent<Image>().color = Color.white;
            }
            else
            {
                ResIcon.GetComponent<Image>().color = Color.white/3f;
            }
        }
    }

    public void Init(CardStats.Resources res)
    {
        ResIcon.Init(res, transform);
        Payed = false;
    }

}
