using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_UI_InteractionWheel : MonoBehaviour
{
    public D_UI_Interaction pInteractionButton;
    public Image mOverlay;
    public float mButtonRadius = 1f;

    List<D_UI_Interaction> mInteractionButtons = new List<D_UI_Interaction>();
    List<D_Interaction> mCurrentInteractions;

    bool bShowing;

    void Start()
    {
        HideInteractions();
    }

    void Update()
    {
        transform.forward = -Camera.main.transform.forward;
    }

    public void ShowInteractions(D_ITargetable target)
    {
        if(bShowing)
        {
            HideInteractions();
        }

        bShowing = true;
        mOverlay.gameObject.SetActive(bShowing);

        mInteractionButtons =  new List<D_UI_Interaction>();
        mCurrentInteractions = target.GetInteractions();

        transform.position = target.GetTransform().position; // - Camera.main.transform.forward;

        foreach(D_Interaction interaction in mCurrentInteractions)
        {
            if(interaction == null)
            {
                continue;
            }

            D_UI_Interaction button = Instantiate(pInteractionButton);
            button.transform.SetParent(transform);
            button.transform.localScale = Vector3.one;
            button.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            button.transform.localPosition = Vector3.zero;

            button.mContainedInteraction = interaction;
            button.mText.text = interaction.mName;
            mInteractionButtons.Add(button);
        }

        // position buttons
        if (mInteractionButtons.Count > 0)
        {
            float degreePerButton = 360f / mInteractionButtons.Count;
            int i = 0;
            foreach (D_UI_Interaction button in mInteractionButtons)
            {
                
                button.transform.localPosition = new Vector3(mButtonRadius * Mathf.Sin(i * degreePerButton * Mathf.Deg2Rad), mButtonRadius * Mathf.Cos(i * degreePerButton * Mathf.Deg2Rad), 0f);
                i++;
            }
        }

    }

    public void HideInteractions()
    {
        if (!bShowing) return;

        bShowing = false;
        mOverlay.gameObject.SetActive(bShowing);
        foreach (D_UI_Interaction button in mInteractionButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
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
