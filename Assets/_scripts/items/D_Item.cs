using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_Item : MonoBehaviour, D_ITargetable
{
    public D_IInventory mStashedInInventory;
    public Sprite mInventorySprite;

    public int mParry = 2;
    public int mIntegrity = 100;

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

    public virtual void InteractPrimary(D_CharacterControl cntl)
    {
        Debug.LogWarning("InteractPrimary not implemented for " + name);
    }

    public virtual void InteractSecondary(D_CharacterControl cntl)
    {
        Debug.LogWarning("InteractSecondary not implemented for " + name);
    }

    public void RemoveFromInventory(D_IInventory from)
    {
        if (from != null)
        {
          //  Debug.Log("from: " + from + " removed!");
            from.RemoveFromInventory(this);
            mStashedInInventory = null;
        }
    }

    public void AddToInventory(D_IInventory to)
    {
        mStashedInInventory = to;
        to.AddToInventory(this);
    }

    public void SwitchInventory(D_IInventory from, D_IInventory to)
    {
       // Debug.Log("from: " + from + " to: " + to);
        RemoveFromInventory(from);
        AddToInventory(to);
    }

    void Start()
    {
        RegisterWithGameMaster();
    }

    void OnDestroy()
    {
        RemoveFromInventory(mStashedInInventory);
        UnregisterFromGameMaster();
    }

    public void RegisterWithGameMaster()
    {
        D_GameMaster.GetInstance().RegisterTargetable(this);
    }

    public void UnregisterFromGameMaster()
    {
        D_GameMaster.GetInstance().UnregisterTargetable(this);
    }
}
