﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class D_Structure : MonoBehaviour, D_ITargetable, IPointerClickHandler
{
    public int mParry     = 2;
    public int mIntegrity = 100;

    public D_Interaction mTargetedByInteraction;

    // === Get / Set by Interface ==
    public Transform GetTransform() { return transform; }

    public int GetParry() { return mParry; }
    public int GetIntegrity() { return mIntegrity; }
    public virtual void SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity <= 0)
        {
            Destroy(gameObject);
        }
    }

    public D_StructsAndEnums.EFaction mFaction;

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

        mTargetedByInteraction = Instantiate(interaction);
        mTargetedByInteraction.transform.SetParent(cntl.transform);

        mTargetedByInteraction.ExecuteInteraction(cntl.mCharacter, this);
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
