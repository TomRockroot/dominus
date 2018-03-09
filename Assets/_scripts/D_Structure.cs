using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using D_StructsAndEnums;

public class D_Structure : MonoBehaviour, D_ITargetable, IPointerClickHandler
{
    public string mName = "Some Structure";

    public int mParry     = 2;
    public int mIntegrity = 100;

    public D_Interaction mTargetedByInteraction;
    [HideInInspector]
    public EInteractionRestriction mRestrictionFlags = EInteractionRestriction.IR_World;

    // === Get / Set by Interface ==
    public string GetName()    { return mName; }

    public D_Maslow GetOnConsumptionMaslow() { return null; }
    public Transform GetTransform() { return transform; }

    public int GetParry() { return mParry; }
    public int GetIntegrity() { return mIntegrity; }
    public virtual int SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity <= 0)
        {
            Destroy(gameObject);
        }
        return mIntegrity;
    }

    public EFaction mFaction;

    void Start()
    {
        RegisterWithGameMaster();
    }

    void OnDestroy()
    {
        UnregisterFromGameMaster();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        D_GameMaster.GetInstance().GetCurrentController().PrepareTarget(this);
    }


    public List<D_Interaction> mPossibleInteractions;
    public List<D_Interaction> GetInteractions() { return mPossibleInteractions; }

    public void Interact(D_CharacterControl cntl, D_Interaction interaction)
    {
        D_Interaction foundInteraction = null;
        foreach (D_Interaction possibleInt in mPossibleInteractions)
        {
            if (possibleInt == interaction)
            {
                foundInteraction = possibleInt;
                break;
            }
        }

        if (foundInteraction == null)
        {
            Debug.LogError("Moppelkotze!");
            return;
        }

        mTargetedByInteraction = interaction;

        mTargetedByInteraction.ExecuteInteraction(cntl.mCharacter, this);
    }

    public void ClearTargetedByInteraction()
    {
        mTargetedByInteraction = null;
    }

    public bool IsInteractionAllowed(D_CharacterControl cntl, EInteractionRestriction restriction)
    {
        return IsFlagged(restriction); 
    }

    public bool IsFlagged(EInteractionRestriction flag)
    {
        return (mRestrictionFlags & flag) == flag;
    }

    public void RegisterWithGameMaster()
    {
        D_GameMaster.GetInstance().RegisterTargetable(this);
    }

    public void UnregisterFromGameMaster()
    {
        if(D_GameMaster.GetInstance() != null)
            D_GameMaster.GetInstance().UnregisterTargetable(this);
    }

    public float GetInteractionRange()
    {
        return 1f;
    }

    public float GetInteractionSpeed()
    {
        return 1f;
    }
}
