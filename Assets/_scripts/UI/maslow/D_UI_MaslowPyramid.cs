using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using D_StructsAndEnums;


public class D_UI_MaslowPyramid : D_UI_Window
{
    public Image mOverlayClosed;
    public Image mOverlayOpen;
    public Text mHappinessText;

    public List<D_UI_MaslowSlot> mSlotsPhysis;
    public List<D_UI_MaslowSlot> mSlotsSafety;
    public List<D_UI_MaslowSlot> mSlotsLove;
    public List<D_UI_MaslowSlot> mSlotsEsteem;
    public List<D_UI_MaslowSlot> mSlotsActuality;

    public override void Open(D_ITargetable target)
    {
        if (target is D_Character)
        {
            OpenPyramid(target as D_Character);
        }
    }

    public override void Close(D_ITargetable target)
    {
        ClosePyramid();
    }

    public void SetHappiness(float happiness)
    {
        mHappinessText.text = ( Mathf.FloorToInt(happiness)).ToString();
    }

    protected void OpenPyramid(D_Character character)
    {
        mOverlayClosed.gameObject.SetActive(false);
        mOverlayClosed.gameObject.SetActive(true);
    }

    protected void ClosePyramid()
    {

    }
}
