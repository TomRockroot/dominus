using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;
using System;

public class D_GameMaster : MonoBehaviour, D_IInventory
{
    [HideInInspector]
    public EDebugLevel mDebugFlags = 0;

    private List<D_ITargetable> mAllTargetables = new List<D_ITargetable>();
    private D_PlayerControl mCurrentController;

    private List<D_Item> mWorldInventory = new List<D_Item>();

    public float mGameMoveSpeed = 1.0f;

    public bool IsFlagged(EDebugLevel flag)
    {
        return (mDebugFlags & flag) == flag;
    }

    public float GetGameMoveSpeed()
    {
        return mGameMoveSpeed;
    }

    public void SetCurrentController(D_PlayerControl controller)
    {
        mCurrentController = controller;
    }

    public D_PlayerControl GetCurrentController()
    {
        return mCurrentController;
    }


    public void RegisterTargetable(D_ITargetable targetable)
    {
        if(mAllTargetables.Contains(targetable))
        {
            Debug.LogError("Tried to add ITargetable " + targetable.GetTransform().name + " twice!\nDENIED!");
            return;
        }
        mAllTargetables.Add(targetable);
      //  Debug.LogWarning("AVAILABLE TARGETS: " + mAllTargetables.Count);
    }

    public void UnregisterTargetable(D_ITargetable targetable)
    {
        if (!mAllTargetables.Contains(targetable))
        {
            Debug.LogError("Tried to remove ITargetable " + targetable.GetTransform().name + " when it's not there anymore!\nDENIED!");
            return;
        }
        mAllTargetables.Remove(targetable);
      //  Debug.LogWarning("AVAILABLE TARGETS: " + mAllTargetables.Count);
    }

    public List<D_ITargetable> GetAllTargetables()
    {
        mAllTargetables.RemoveAll(D_ITargetable => D_ITargetable == null);
        return mAllTargetables;
    }

    // ==== SINGLETON SHIT ====
    private static D_GameMaster GAME_MASTER;

    public static D_GameMaster GetInstance()
    {
        if(GAME_MASTER == null)
        {
            GAME_MASTER = FindObjectOfType<D_GameMaster>();
        }
        return GAME_MASTER; 
    }

    public void RemoveFromInventory(D_Item item)
    {
        mWorldInventory.Remove(item);
    }

    public void AddToInventory(D_Item item)
    {
        mWorldInventory.Add(item);
    }

    public void DropInventory()
    {
        throw new NotImplementedException();
    }

    public List<D_Item> GetInventory()
    {
        return mWorldInventory;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
