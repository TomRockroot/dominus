using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_UI_InventorySlot : MonoBehaviour, IPointerClickHandler
{

    public D_Item mContainedItem;

    public Image mFrame;
    public Image mItemImage;

    public void AssignItem(D_Item item)
    {
        mContainedItem = item;
        mItemImage.sprite = item.GetData().mInventorySprite;
        mItemImage.color = Color.white;
    }

    public void ClearItem()
    {
        mItemImage.color = Color.clear;
        mContainedItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mContainedItem != null)
        {
            D_GameMaster.GetInstance().GetCurrentController().PrepareTarget(mContainedItem, EInteractionRestriction.IR_Inventory);
        }
    }
}
