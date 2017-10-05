using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_GameMaster : MonoBehaviour {

    private List<D_ITargetable> mAllTargetables = new List<D_ITargetable>();

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
}
