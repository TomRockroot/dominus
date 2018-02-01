using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_UI_Manager : MonoBehaviour
{
    public EUserInterface mState;
    protected D_UI_Window mCurrentWindow;

    public D_UI_Culture       mCultureUI;
    public D_UI_Inventory     mInventoryUI;
    public D_UI_Equipment     mEquipmentUI;
    public D_UI_MaslowPyramid mMaslowUI;

    public D_UI_CraftMenu     mCraftUI;
    public D_UI_Statistics    mStatisticsUI;
    public D_UI_Dialog        mDialogUI;
    public D_UI_Menu          mMenuUI;


    public void OpenWindow(EUserInterface state, D_ITargetable target)
    {
        Debug.Log("UI: " + mState + " ~> " + state);

        mCultureUI.Close(target);
        mInventoryUI.Close(target);
        mEquipmentUI.Close(target);
        mMaslowUI.Close(target);

        mCraftUI.Close(target);
        mStatisticsUI.Close(target);
        mDialogUI.Close(target);
        mMenuUI.Close(target);


        switch (state)
        {
            case EUserInterface.UI_Culture:
                mCultureUI.Open(target);
                break;
            case EUserInterface.UI_Inventory:
                mInventoryUI.Open(target);
                break;
            case EUserInterface.UI_Equipment:
                mEquipmentUI.Open(target);
                break;
            case EUserInterface.UI_Maslow:
                mMaslowUI.Open(target);
                break;

            case EUserInterface.UI_Craft:
                mCraftUI.Open(target);
                break;
            case EUserInterface.UI_Statistics:
                mStatisticsUI.Open(target);
                break;
            case EUserInterface.UI_Dialog:
                mDialogUI.Open(target);
                break;
            case EUserInterface.UI_Menu:
                mMenuUI.Open(target);
                break;
            default:
                if(D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_UI_Warning)) Debug.LogWarning("UI(" + name + "): Received state " + state + "... defaulted!");
                break;
        }

        mState = state;
    }

    public void CloseWindows()
    {
        OpenWindow(EUserInterface.UI_None, null);
    }

    // ==== SINGLETON SHIT ====
    private static D_UI_Manager UI_MANAGER;

	public static D_UI_Manager GetInstance()
    {
        if(UI_MANAGER == null)
        {
            UI_MANAGER = FindObjectOfType<D_UI_Manager>();
        }
        return UI_MANAGER;
    }
}
