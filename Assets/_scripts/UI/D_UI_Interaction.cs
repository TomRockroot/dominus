using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class D_UI_Interaction : MonoBehaviour, IPointerClickHandler
{
    public D_Interaction mContainedInteraction;
    public Text mText;

    public void OnPointerClick(PointerEventData eventData)
    {
        //mContainedInteraction
        Debug.Log(mText.text + " was clicked!");

        D_PlayerControl cntl = D_GameMaster.GetInstance().GetCurrentController();
        D_ITargetable target = cntl.GetPreparedTarget();

        target.Interact(cntl, mContainedInteraction);
    }
}
