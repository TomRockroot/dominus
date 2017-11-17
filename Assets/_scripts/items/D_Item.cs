using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class D_Item : MonoBehaviour, D_ITargetable, IPointerClickHandler
{
    public D_IInventory mStashedInInventory;
    public Sprite mInventorySprite;

    public int mParry = 2;
    public int mIntegrity = 100;

    public D_Interaction mTargetedByInteraction;

    // === Get / Set by Interface ==
    public Transform GetTransform() { return transform; }

    public int GetParry() { return mParry; }
    public int GetIntegrity() { return mIntegrity; }
    public void SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity < 0)
        {
            Destroy(gameObject);
        }
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


        mTargetedByInteraction = Instantiate(interaction);
        mTargetedByInteraction.transform.SetParent(cntl.transform);

        mTargetedByInteraction.ExecuteInteraction(cntl.mCharacter, this);
    }


    public void RemoveSelfFromInventory(D_IInventory from)
    {
        if (from != null)
        {
            if (!from.Equals(null))
            {
                // Debug.LogError("from: " + from + " removed! Type: " + from.GetType());
                from.RemoveFromInventory(this);
                mStashedInInventory = null;
            }
        }
    }

    public void AddSelfToInventory(D_IInventory to)
    {
        mStashedInInventory = to;
        to.AddToInventory(this);
        transform.parent = to.GetTransform();
        transform.localPosition = Vector3.zero;
    }

    public void SwitchSelfInventory(D_IInventory from, D_IInventory to)
    {
       // Debug.Log("from: " + from + " to: " + to);
        RemoveSelfFromInventory(from);
        AddSelfToInventory(to);
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
