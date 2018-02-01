using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class D_UI_Inventory : D_UI_Window
{
    public Vector3 mButtonOffset;

    public bool bOpen;

    public Image mBackgroundOpen;

    public D_UI_InventorySlot pSlot;
    public List<D_UI_InventorySlot> mSlots;

    public D_IInventory mCurrentPossessor;

    public override void Open(D_ITargetable target)
    {
        if (target is D_IInventory)
        {
            OpenInventory(target as D_IInventory);
        }
    }

    public override void Close(D_ITargetable target)
    {
        mCurrentPossessor = null;
        ClearInventory();

        foreach (D_UI_InventorySlot slot in mSlots)
        {
            slot.mItemImage.enabled = false;
            slot.mFrame.enabled = false;
        }

        mBackgroundOpen.GetComponent<Image>().enabled = false;
    }

    protected void ClearInventory()
    {
        foreach (D_UI_InventorySlot slot in mSlots)
        {
            slot.ClearItem();
        }
    }

    public void RemoveItem(D_Item item)
    {
        foreach (D_UI_InventorySlot slot in mSlots)
        {
            if (slot.mContainedItem == item)
            {
                slot.ClearItem();
            }
        }
    }

    public void AddItem(D_Item item)
    {
        foreach (D_UI_InventorySlot slot in mSlots)
        {
            if (slot.mContainedItem == item)
            {
                break;
            }

            if (slot.mContainedItem == null)
            {
                slot.AssignItem(item);
                break;
            }
        }
    }

    protected void OpenInventory(D_IInventory possessor)
    {
        bOpen = true;

        if (mCurrentPossessor == possessor)
        {
            Debug.Log("Sup yo!");
        }

        ClearInventory();
        mCurrentPossessor = possessor;
        List<D_Item> inventory = possessor.GetInventory();

        foreach(D_Item item in inventory)
        {
            AddItem(item);
        }

        foreach (D_UI_InventorySlot slot in mSlots)
        {
            slot.mItemImage.enabled = true;
            slot.mFrame.enabled = true;
        }

        mBackgroundOpen.GetComponent<Image>().enabled = true;
    }
}
