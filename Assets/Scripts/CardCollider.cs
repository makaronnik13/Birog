using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollider : MonoBehaviour
{
    public Action OnClicked = ()=> { };
    public Action<bool> OnFocusChanged = (b)=> { };


    void OnMouseDown()
    {
        OnClicked();     
    }

    private void OnMouseDrag()
    {

        // Debug.Log("Drag");
    }

    private void OnMouseEnter()
    {
        OnFocusChanged(true);
    }

    private void OnMouseExit()
    {

        OnFocusChanged(false);
    }

    private void OnMouseUp()
    {
 
        //Debug.Log("Up");
    }


}
