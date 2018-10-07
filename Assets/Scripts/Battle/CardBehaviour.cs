using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour {

    private Billboard _billboard;
    private static float _movementSpeed = 0.1f;
    private static float _scaleSpeed = 1f;

    private bool _focused = false;

 
    private bool _open = true;
    private Action _callback;

    public bool Open
    {
        get
        {
            return _open;
        }
        set
        {
            if (_open!=value)
            {
                _open = value;
                Reposition();
            }
        }
    }

    public bool Focused
    {
        get
        {
            return _focused;
        }
        set
        {
            if (_focused == value)
            {
                return;
            }
            _billboard.enabled = value;
            _focused = value;
            Reposition();
        }
    }

    public Vector2 Size
    {
        get
        {
            return GetComponentInChildren<BoxCollider>().transform.localScale;
        }
    }

    private void Reposition()
    {
        CardsLayout layout = GetComponentInParent<CardsLayout>();
        if (layout)
        {
            Reposition(layout);
        }
    }

    private void Start()
    {
        GetComponentInChildren<CardCollider>().OnClicked += CardClicked;
        GetComponentInChildren<CardCollider>().OnFocusChanged += FocusChanged;
        _billboard = gameObject.AddComponent<Billboard>();
        _billboard.enabled = false;
    }

    private void FocusChanged(bool val)
    {
        Focused = val;

        /*
        if (val)
        {
            CardsLayoutManager.Instance.FocusedCard = this;
        }
        else
        {
            CardsLayoutManager.Instance.FocusedCard = null;
        }
        */
    }

    private void CardClicked()
    {
        
    }

    public void Reposition(CardsLayout layout, Action callback = null)
    {
        Vector3 scale = Vector3.one;
 
        MoveCardTo(layout, callback);
    }

    public Vector3 GetPosition(CardsLayout layout)
    {
        if (!layout)
        {
            return transform.position;
        }

        return layout.GetPosition(this);
    }

    public Quaternion GetRotation(CardsLayout layout)
    {
        if (!layout)
        {
            return transform.localRotation;
        }

        return layout.GetRotation(this);
    }

    public void MoveCardTo(CardsLayout layout, Action callback = null)
    {
        if (transform.parent != null && transform.parent.GetComponent<CardsLayout>())
        {
           // transform.parent.GetComponent<CardsLayout>().RemoveCardFromLayout(this);
        }

        //layout.AddCardToLayout(this);
        MoveCardTo(layout, Vector3.one, callback);
    }

    private void MoveCardTo(CardsLayout parent, Vector3 localScale, Action callback = null)
    {
        Vector3 localPosition = GetPosition(parent);
        Quaternion localRotation = GetRotation(parent);

        StopAllCoroutines();
        StartCoroutine(MoveCardToCoroutine(parent.transform, localPosition, localRotation, localScale, callback));
    }

    private void OnDestroy()
    {
        if (GetComponentInParent<CardsLayout>())
        {
            GetComponentInParent<CardsLayout>().RemoveCardFromLayout(this);
        }
    }

    private IEnumerator MoveCardToCoroutine(Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Action callback = null)
    {
        if (callback!=null)
        {
            _callback = callback;
        }
       
        transform.SetParent(parent);
        float time = 0.0f;
        float speed = Mathf.Max(_movementSpeed, _scaleSpeed);

        while (time < speed)
        {
            if (time < _scaleSpeed)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, localScale, time / _scaleSpeed);
            }

            if (time < _movementSpeed)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, time / _movementSpeed);
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, localRotation, time / speed);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }


        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        transform.localScale = localScale;

        if (_callback != null)
        {
            Debug.Log("With callback!");
            _callback.Invoke();
            _callback = null;
        }
    }
}
