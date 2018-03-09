using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_Item : MonoBehaviour, D_ITargetable, IPointerClickHandler
{
    public string mName = "Some Item";
    public bool bHidden = false;

    public D_IInventory mStashedInInventory;
    public Sprite mInventorySprite;

    public int mParry = 2;
    public int mIntegrity = 100;

    public D_Interaction mTargetedByInteraction;

    [HideInInspector]
    public EInteractionRestriction mRestrictionFlags = EInteractionRestriction.IR_World;

    public D_Maslow mOnConsumptionMaslow;

    // === Get / Set by Interface ==
    public string GetName() { return mName; }

    public D_Maslow GetOnConsumptionMaslow() { return mOnConsumptionMaslow; }
    public Transform GetTransform() { return transform; }

    public int GetParry() { return mParry; }
    public int GetIntegrity() { return mIntegrity; }
    public int SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity < 0)
        {
            Destroy(gameObject);
        }
        return mIntegrity;
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
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Item_Error)) Debug.LogError("Moppelkotze! \n" + cntl.name + " could not do " + interaction.name + " on " + name );
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
        if(!IsFlagged( restriction ) )
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Item_Message)) Debug.Log("Interaction on " + name + " was not allowed \ndue to " + restriction + " while placed at " + mRestrictionFlags);
            return false;
        }

        if(mTargetedByInteraction == null)
        {
            if(bHidden)
            {
                if(mStashedInInventory is D_Character)
                {
                    D_Character possessor = mStashedInInventory as D_Character;
                    if(possessor == cntl.mCharacter)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // DESIGN CHOICE! (If Object is hidden on map, but -say- in a house, I want to be able to interact f.e. to pick it up and get it out of there)
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Warning) ) { Debug.LogWarning("Item: " + cntl.name + " is not allowed to interact with " + name + "\nbecause it's already targeted by " + mTargetedByInteraction.mName + " - Time: " + Time.time); }
            return false;
        }
    }

    public bool IsFlagged(EInteractionRestriction flag)
    {
        return (mRestrictionFlags & flag) == flag;
    }

    public void SetFlag(EInteractionRestriction flag)
    {
        mRestrictionFlags = mRestrictionFlags | flag;
    }

    public void ClearFlags()
    {
        mRestrictionFlags = 0;
    }
        

    public void RemoveSelfFromInventory(D_IInventory from)
    {
        if (from != null)
        {
            if (!from.Equals(null))
            {
                from.RemoveFromInventory(this);
                mStashedInInventory = null;
                Hide(false);
            }
        }
    }

    public void AddSelfToInventory(D_IInventory to, bool resetPos = true)
    {
        mStashedInInventory = to;
        to.AddToInventory(this);

        //SPAGGETHI ToDo: un-spaggeddi this
        if(!resetPos)
        {
            mRestrictionFlags = EInteractionRestriction.IR_World;
        }
        else
        {
            mRestrictionFlags = EInteractionRestriction.IR_Inventory;
        }

        if (resetPos)
        {
            transform.parent = to.GetTransform();
            transform.localPosition = Vector3.zero;
            Hide(true);
        }
    }

    public void SwitchSelfInventory(D_IInventory from, D_IInventory to)
    {
       // Debug.Log("from: " + from + " to: " + to);
        RemoveSelfFromInventory(from);
        AddSelfToInventory(to);
    }

    public void Hide(bool value)
    {
        bHidden = value;
        GetComponent<MeshRenderer>().enabled = !value;
        GetComponent<Collider>().enabled = !value;
    }

    void Start()
    {
        RegisterWithGameMaster();
    }

    void OnDestroy()
    {
        RemoveSelfFromInventory(mStashedInInventory);
        UnregisterFromGameMaster();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        D_GameMaster.GetInstance().GetCurrentController().PrepareTarget(this);
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
