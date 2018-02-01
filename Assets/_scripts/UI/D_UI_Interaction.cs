using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using D_StructsAndEnums;

public class D_UI_Interaction : MonoBehaviour, IPointerClickHandler
{
    public D_Interaction mContainedInteraction;
    public Text mText;

    public Sprite mNotImplemented;

    public void SetImplemented(bool imp = true)
    {
        if(!imp)
            GetComponent<Image>().sprite = mNotImplemented;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        D_SoundMaster.GetInstance().PlaySound(D_StructsAndEnums.ESoundCue.SC_ClickUI, 0);
        Debug.Log(mText.text + " was clicked!");

        D_PlayerControl cntl = D_GameMaster.GetInstance().GetCurrentController();
        D_ITargetable target = cntl.GetPreparedTarget();

        Camera.main.GetComponent<D_CameraFollow>().SetCameraState(ECameraMode.CM_Follow, D_GameMaster.GetInstance().GetCurrentController().transform);
        target.Interact(cntl, mContainedInteraction);
    }
}
