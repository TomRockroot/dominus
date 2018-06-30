using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_UI_CraftSlot : MonoBehaviour
{
    public ECraftSlot mSlotID;

    public Text mItemName;
    public Text mNumbers;

    public Image mItemImage;
    public Image mFrame;

    public Sprite mAvailable;
    public Sprite mNotAvailable;

    public D_Item mCurrentPrefab;
    public List<D_Item> mSimilarItems = new List<D_Item>();

    public bool SetSlot(D_Recipe recipe, D_ICrafter crafter)
    {
        int amount = 0;
        int available = 0;

        switch (mSlotID)
        {
            case ECraftSlot.CS_SlotOne:
                mCurrentPrefab = recipe.pPart1;
                amount = recipe.mAmountNeeded1;
                break;
            case ECraftSlot.CS_SlotTwo:
                mCurrentPrefab = recipe.pPart2;
                amount = recipe.mAmountNeeded2;
                break;
        }

        mItemName.text = mCurrentPrefab.GetName();
        mItemImage.sprite = mCurrentPrefab.GetData().mInventorySprite;

        mSimilarItems = crafter.GetSimilarItems(mCurrentPrefab);
        available = mSimilarItems.Count;
        mNumbers.text = available + " / " + amount;

        if(amount <= available)
        {
            mFrame.sprite = mAvailable;
            return true;
        }

        mFrame.sprite = mNotAvailable;
        return false;
    }

    public void SetResult(D_Recipe recipe, D_ICrafter crafter, bool viable)
    {
        int amount = 0;

        mCurrentPrefab = recipe.pResult;
        amount = recipe.mAmountYield;

        mItemName.text = mCurrentPrefab.GetName();
        mItemImage.sprite = mCurrentPrefab.GetData().mInventorySprite;
        
        mNumbers.text = amount + "++ (" + crafter.GetSimilarItems(mCurrentPrefab).Count + ")";

        if(viable)
        {
            mFrame.sprite = mAvailable;
        }
        else
        {
            mFrame.sprite = mNotAvailable;
        }
    }
}
