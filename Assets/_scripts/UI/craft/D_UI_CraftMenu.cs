using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class D_UI_CraftMenu : D_UI_Window
{
    public Image mBackground;

    public D_UI_CraftSlot mPartSlot1;
    public D_UI_CraftSlot mPartSlot2;
    public D_UI_CraftSlot mResultSlot;

    public Button mCraftButton;

    public D_ICrafter mCurrentCrafter;
    public D_Recipe   mActiveRecipe;
    public List<D_Recipe> mCurrentRecipes;

    public override void Open(D_ITargetable target)
    {
        if (target is D_ICrafter)
        {
            OpenCraft(target as D_ICrafter);
        }
        else
        {
            if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_UI_Error)) Debug.LogError("UI Craft: " + target.GetTransform().name + " is not a D_ICrafter!");
        }
    }

    public override void Close(D_ITargetable target)
    {
        CloseCraft();
    }

    public void OpenCraft(D_ICrafter crafter)
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.name + " was activated");
            child.gameObject.SetActive(true);
        }

        mCurrentCrafter = crafter;
        mCurrentRecipes = crafter.GetRecipes();
        
        foreach(D_Recipe recipe in mCurrentRecipes)
        {
            SetRecipe(recipe, crafter);
            break;
        }
    }

    public void CloseCraft()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        mCurrentCrafter = null;
        mCurrentRecipes = null;
    }

    protected void SetRecipe(D_Recipe recipe, D_ICrafter crafter)
    {
        mActiveRecipe = recipe;
        bool viable = true;

        /* Kommen drei Logiker in eine Bar,
            Barkeeper: "Ihr wollt dich sicher alle drei je ein Bier!"
            Logiker 1: "Weiß ich nicht ..."
            Logiker 2: "Weiß ich nicht ..."
            Logiker 3: "Ja!"
        */

        viable = mPartSlot1.SetSlot(recipe, crafter) && viable;
        viable = mPartSlot2.SetSlot(recipe, crafter) && viable;

        mResultSlot.SetResult(recipe, crafter, viable);

        // DON'T DO THIS:  viable = viable && mPartSlot1.SetSlot(recipe, crafter);
        // It will not execute "SetSlot" ... ??

        mCraftButton.interactable = viable;
    }

    public void Craft()
    {
        Debug.Log(mPartSlot1.mCurrentPrefab.GetName() + " + " + mPartSlot2.mCurrentPrefab.GetName() + " => " + mResultSlot.mCurrentPrefab.GetName());

        int i = 1;
        foreach(D_Item item in mPartSlot1.mSimilarItems)
        {
            Destroy(item.gameObject);
            i++;
            if(i > mActiveRecipe.mAmountNeeded1)
            {
                break;
            }
        }

        int j = 1;
        foreach (D_Item item in mPartSlot2.mSimilarItems)
        {
            Destroy(item.gameObject);
            j++;
            if (j > mActiveRecipe.mAmountNeeded2)
            {
                break;
            }
        }

        int k = 0;
        for (k= 0; k < mActiveRecipe.mAmountYield; k++)
        {
            D_Item craftedItem = Instantiate(mActiveRecipe.pResult, mCurrentCrafter.GetTransform());
            craftedItem.AddSelfToInventory(mCurrentCrafter.AsInventory());
            craftedItem.Hide(true);
        }
    }
}
