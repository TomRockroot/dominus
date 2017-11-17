using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class D_UI_InventorySlot : MonoBehaviour, IPointerClickHandler
{

    public D_Item mContainedItem;

    public Image mFrame;
    public Image mItemImage;

    public void AssignItem(D_Item item)
    {
        mContainedItem = item;
        mItemImage.sprite = item.mInventorySprite;
        mItemImage.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mContainedItem != null)
        {
            D_GameMaster.GetInstance().GetCurrentController().PrepareTarget(mContainedItem);
        }
    }
}
