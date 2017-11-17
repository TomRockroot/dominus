using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class D_UI_Inventory : MonoBehaviour, IPointerClickHandler
{
    public Vector3 mButtonOffset;

    protected bool bOpen;

    public Image mBackgroundOpen;
    public Image mBackgroundClosed;

    public D_UI_InventorySlot pSlot;
    public List<D_UI_InventorySlot> mSlots;

    void Start()
    {
        SwitchInventory(false);
    }

    public void UpdateUI(List<D_Item> inventory)
    {
        foreach (D_Item item in inventory)
        {
            foreach (D_UI_InventorySlot slot in mSlots)
            {
                if(slot.mContainedItem == item)
                {
                    break;
                }

                if(slot.mContainedItem == null)
                {
                    slot.AssignItem(item);
                    break;
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchInventory(!bOpen);
    }

    protected void SwitchInventory(bool open)
    {
        bOpen = open;

        foreach (D_UI_InventorySlot slot in mSlots)
        {
            slot.mItemImage.enabled = bOpen;
            slot.mFrame.enabled = bOpen;
        }
        mBackgroundOpen.GetComponent<Image>().enabled = bOpen;
        mBackgroundClosed.GetComponent<Image>().enabled = !bOpen;
    }
}
