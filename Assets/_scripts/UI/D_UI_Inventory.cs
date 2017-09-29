using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_UI_Inventory : MonoBehaviour
{
    public Vector3 mButtonOffset;

    public Transform pSlot;
    public List<Transform> mSlots;

    public void UpdateUI(List<D_Item> inventory)
    {
        int i = 0;

        foreach (D_Item item in inventory)
        {
            if(mSlots.Count < i+1)
            {
                mSlots.Add(Instantiate(pSlot, mSlots[i - 1].position + mButtonOffset, Quaternion.identity, transform));
            }
            else if(mSlots[i] == null)
            {
                mSlots[i] = Instantiate(pSlot, mSlots[i-1].position + mButtonOffset, Quaternion.identity, transform);
            }
            
            item.GetComponent<Renderer>().enabled = false;

            mSlots[i].GetComponent<Image>().sprite = item.mInventorySprite;

            i++;
        }
    }
}
