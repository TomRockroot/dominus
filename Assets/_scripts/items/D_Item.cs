using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_Item : MonoBehaviour, D_ITargetable, IPointerClickHandler
{
    [SerializeField]
    protected D_ItemData mData;

    public D_ItemData GetData()
    {
        return mData;
    }

    public void SetData(D_ItemData data)
    {
        mData = data;
        UpdateMaterial();
    }
    
    protected A_Node mNode;

    public int mCurrentIntegrity;
    [HideInInspector]
    public EInteractionRestriction mRestrictionFlags = EInteractionRestriction.IR_World;

    // ==============================
    // ===== GET/SET INTERFACE  =====
    // ==============================
    public string GetName() { return GetData().mName; }

    public D_Maslow GetOnConsumptionMaslow() { return GetData().mOnConsumptionMaslow; }
    public Transform GetTransform() { return transform; }

    public int GetParry() { return GetData().mParry; }
    public int GetIntegrity() { return GetData().mIntegrity; }
    public int SetIntegrity(int integrity)
    {
        mCurrentIntegrity = integrity;
        if (mCurrentIntegrity < 0)
        {
            Destroy(gameObject);
        }
        return mCurrentIntegrity;
    }

    public float GetInteractionRange()  { return GetData().mInteractionRange; }
    public float GetInteractionSpeed()  { return GetData().mInteractionSpeed; }

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
    protected bool bHidden = false;
    protected D_Interaction mTargetedByInteraction;

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


    // ==============================
    // =====     INVENTORY      =====
    // ==============================
    protected D_IInventory mStashedInInventory;

    public D_IInventory GetStashedInInventory() { return mStashedInInventory; }

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

    // ==============================
    // =====  UNITY CALLBACKS   =====
    // ==============================
    void Start()
    {
        RegisterWithGameMaster();
        mCurrentIntegrity = GetData().mIntegrity;

        A_Grid.SnapToGrid(this, D_GameMaster.GetInstance().GetCurrentLevel().mGrid);
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

    void OnValidate()
    {
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (mData != null)
        {
            if (GetComponent<Renderer>().sharedMaterial != mData.mBillboard)
            {
                GetComponent<Renderer>().sharedMaterial = mData.mBillboard;
            }
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = null;
        }
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
