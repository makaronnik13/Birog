using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    private CardCollider _collider;
    private static float _movementSpeed = 3000f;
    private static float _rotationSpeed = 3000f;
    private Billboard _billboard;
    private bool _focused = false;
    private bool _open = true;
    private Action _callback = ()=>{};

    public CardsLayout Parent;
   
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
        }
    }
    public Vector2 Size
    {
        get
        {
            return GetComponentInChildren<BoxCollider>().transform.localScale;
        }
    }


    private void Start()
    {
        _collider = GetComponentInChildren<CardCollider>();
        _collider.OnClicked += CardClicked;
        _collider.OnFocusChanged += FocusChanged;
        _billboard = gameObject.AddComponent<Billboard>();
        _billboard.enabled = false;
    }

    private void FocusChanged(bool val)
    {
        Focused = val;
    }

    private void CardClicked()
    {
        
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

   
    private void OnDestroy()
    {
        if (Parent)
        {
            Parent.RemoveCardFromLayout(this);
        }
    }

    private void Update()
    {
        if (Parent!=null)
        {
            Vector3 aimPos = Vector3.MoveTowards(transform.localPosition, GetPosition(Parent), _movementSpeed * Time.deltaTime);
            Quaternion aimRot = Quaternion.RotateTowards(transform.localRotation, GetRotation(Parent), _rotationSpeed * Time.deltaTime);

            if (transform.localPosition == aimPos)
            {
                if (_callback != null)
                {
                    _callback.Invoke();
                    _callback = null;
                }
            }

            transform.localPosition = aimPos;
            transform.localRotation= aimRot;
        }
    }

    public void AddCallback(Action callback)
    {
        Debug.Log("add callback");
        _callback += callback;
    }
}
