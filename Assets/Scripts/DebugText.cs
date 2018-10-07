using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DebugText : Singleton<DebugText> {

    private TextMeshProUGUI __text;
    private TextMeshProUGUI _text
    {
        get
        {
            if (!__text)
            {
                __text = GetComponent<TextMeshProUGUI>();
            }
            return __text;
        }
    }

	public void Debug(string text)
    {
        _text.text += "/n"+text;
    }
}
