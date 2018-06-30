using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using D_StructsAndEnums;

public class D_Structure : MonoBehaviour, D_ITargetable, IPointerClickHandler
{
    [SerializeField]
    protected D_StructureData mData;

    public D_StructureData GetData() // WHY NO SET? TOM OF THE PAST!!! WHYYYYY?!
    {
        return mData;
    }

    public void SetData(D_StructureData data)
    {
        mData = data;
        mCurrentIntegrity = mData.mIntegrity;
    }

    [SerializeField]
    protected A_Node mNode;

    public int mCurrentIntegrity;
    public D_Interaction mTargetedByInteraction;
    [HideInInspector]
    public EInteractionRestriction mRestrictionFlags = EInteractionRestriction.IR_World;

    // ==============================
    // ===== GET/SET INTERFACE  =====
    // ==============================
    public string GetName()    { return GetData().mName; }

    public D_Maslow GetOnConsumptionMaslow() { return GetData().mOnUseMaslow; }
    public Transform GetTransform() { return transform; }

    public int GetParry() { return GetData().mParry; }
    public int GetIntegrity() { return mCurrentIntegrity; }  
    public virtual int SetIntegrity(int integrity)  // <-- PROBLEM: Sets the integrity of ALL TREEs with the same Data 
    {
        mCurrentIntegrity = integrity;
        if (mCurrentIntegrity <= 0)
        {
            Destroy(gameObject);
        }
        return mCurrentIntegrity;
    }

    public float GetInteractionRange() { return GetData().mInteractionRange; }
    public float GetInteractionSpeed() { return GetData().mInteractionSpeed; }

    // ==============================
    // =====        GRID        =====
    // ==============================
    public A_Node GetNode() { return mNode; }
    public void SetNode(A_Node node)
    {
        if (mNode != null)
        {
            mNode.RemoveOccupant(this);
        } 

        node.AddOccupant(this);

        mNode = node;
    }
    public ENodeStatus GetOccupyType() { return GetData().mOccupyType; }

    // ==============================
    // =====    INTERACTION     =====
    // ==============================
    public List<D_Interaction> GetInteractions() { return GetData().mPossibleInteractions; }

    public void Interact(D_CharacterControl cntl, D_Interaction interaction)
    {
        D_Interaction foundInteraction = null;
        foreach (D_Interaction possibleInt in GetData().mPossibleInteractions)
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

    // ==============================
    // =====  UNITY CALLBACKS   =====
    // ==============================
    void Start()
    {
        RegisterWithGameMaster();
        if (mData != null)
        {
            mCurrentIntegrity = GetData().mIntegrity;
            Debug.Log("Structure ("+ name +") DataID: " + mData.GetInstanceID());
        }

        A_Grid.SnapToGrid(this, D_GameMaster.GetInstance().GetCurrentLevel().mGrid);
    }

    void OnDestroy()
    {
        UnregisterFromGameMaster();
    }

    void OnValidate()
    {
        Renderer renderer = GetComponent<Renderer>();
        if(renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>(true);
        }

        if (mData != null)
        {
            if (renderer.sharedMaterial != mData.mBillboard)
            {
                renderer.sharedMaterial = mData.mBillboard;
            }
        }
        else
        {
            renderer.sharedMaterial = null;
        }
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
}
