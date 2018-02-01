using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_UI_InteractionWheel : MonoBehaviour
{
    public D_UI_Interaction pInteractionButton;
    public Image mOverlay;
    public Image mInventoryOverlay;

    public float mForwardOffset = 1f;

    public float mButtonRadius = 1f;
    public float mButtonDepth = 2f;
    public float mButtonSingleVerticalOffset = 10f;

    public float mInvButtonRadius = 1f;
    public float mInvCircularOffset = 0f;

    List<D_UI_Interaction> mInteractionButtons = new List<D_UI_Interaction>();
    List<D_Interaction> mCurrentInteractions;

    bool bShowing;

    void Start()
    {
        mOverlay.gameObject.SetActive(false);
        mInventoryOverlay.gameObject.SetActive(false);
    }

    void Update()
    {
    }

    public bool ShowInteractions(D_ITargetable target, EInteractionRestriction restriction = EInteractionRestriction.IR_World)
    {
        if(bShowing)
        {
            HideInteractions();
        }

        bShowing = true;

        if (restriction == EInteractionRestriction.IR_World)
        {
            mOverlay.gameObject.SetActive(bShowing);
        }
        else
        {
            mInventoryOverlay.gameObject.SetActive(bShowing);
        }

        mInteractionButtons =  new List<D_UI_Interaction>();
        mCurrentInteractions = target.GetInteractions();

         //World Space System
        //transform.position = target.GetTransform().position - Camera.main.transform.forward * mForwardOffset; 

        

        foreach(D_Interaction interaction in mCurrentInteractions)
        {
            if(interaction == null)
            {
                continue;
            }

            if(!( (interaction.mRestrictionFlags & restriction) == restriction))
            {

                if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_UI_Message)) Debug.Log("UI_Wheel: '" + interaction.mName + "' has " + interaction.mRestrictionFlags + " while " + restriction + " was needed!");
                continue;
            }    

            D_UI_Interaction button = Instantiate(pInteractionButton);
            if (restriction == EInteractionRestriction.IR_World)
            {
                button.transform.SetParent(transform);
            }
            else
            {
                button.transform.SetParent(mInventoryOverlay.transform);
            }

            button.transform.localScale = Vector3.one;
            button.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            button.transform.localPosition = Vector3.forward * mButtonDepth;

            button.mContainedInteraction = interaction;
            button.mText.text = interaction.mName;
            button.SetImplemented(interaction.bImplemented);
            mInteractionButtons.Add(button);
        }

        // position buttons
        if (mInteractionButtons.Count > 0)
        {
            if (restriction == EInteractionRestriction.IR_World)
            {
                if (mInteractionButtons.Count > 1)
                {
                    float degreePerButton = 360f / mInteractionButtons.Count;
                    int i = 0;
                    foreach (D_UI_Interaction button in mInteractionButtons)
                    {
                        button.transform.localPosition = new Vector3(mButtonRadius * Mathf.Sin(i * degreePerButton * Mathf.Deg2Rad), mButtonRadius * Mathf.Cos(i * degreePerButton * Mathf.Deg2Rad), 0f);
                        i++;
                    }
                }
                else
                {
                    foreach (D_UI_Interaction button in mInteractionButtons)
                    {
                        button.transform.localPosition = Vector3.forward * mButtonDepth + Vector3.up * mButtonSingleVerticalOffset;
                        mOverlay.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                float degreePerButton = 120f / mInteractionButtons.Count;
                int i = 0;
                foreach (D_UI_Interaction button in mInteractionButtons)
                {

                    button.transform.localPosition = new Vector3(mInvButtonRadius * (Mathf.Sin(i * (degreePerButton) * Mathf.Deg2Rad) ), mInvButtonRadius * (Mathf.Cos(i * (degreePerButton) * Mathf.Deg2Rad)), 0f);
                    i++;
                }
            }
        }
        else
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_UI_Message)) Debug.Log("No interactions to show ... Wheel hidden!");
            bShowing = false;
            mOverlay.gameObject.SetActive(bShowing);
            mInventoryOverlay.gameObject.SetActive(bShowing);
        }
        
        D_SoundMaster.GetInstance().PlaySound(ESoundCue.SC_ClickUI, 1);

        return bShowing;
    }

    public void HideInteractions()
    {
        if (!bShowing) return;

        bShowing = false;
        mOverlay.gameObject.SetActive(bShowing);
        mInventoryOverlay.gameObject.SetActive(bShowing);

        foreach (D_UI_Interaction button in mInteractionButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
    }

    public void SetWheelPosition(Vector3 vec)
    {
        transform.position = vec;
    }

    // ==== SINGLETON SHIT ====
    private static D_UI_InteractionWheel INTERACTION_WHEEL;

    public static D_UI_InteractionWheel GetInstance()
    {
        if (INTERACTION_WHEEL == null)
        {
            INTERACTION_WHEEL = GameObject.FindObjectOfType<D_UI_InteractionWheel>();
        }
        return INTERACTION_WHEEL;
    }
}
