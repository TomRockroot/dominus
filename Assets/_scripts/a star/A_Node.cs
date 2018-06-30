using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;


public class A_Node : ScriptableObject
{
    public A_Grid mGrid;
    public ENodeStatus mStatus = ENodeStatus.NS_Normal;

    protected List<D_ITargetable> mOccupants = new List<D_ITargetable>();

    public int x;
    public int y; 

    public void Initialize(int _x, int _y, A_Grid grid)
    {
        x = _x;
        y = _y; 

        mGrid = grid;

        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public A_Grid GetGrid()
    {
        return mGrid;
    }

    public List<D_ITargetable> GetOccupants()
    {
        return mOccupants;
    }

    public void AddOccupant(D_ITargetable occ)
    {
        if(mOccupants.Contains(occ))
        {
            Debug.LogWarning("Tried to add " + occ.GetTransform().name + " to Occupants of node ("+ x + "/"+ y +")");
            return;
        }

        if (occ.GetOccupyType() > mStatus)
        {
            mStatus = occ.GetOccupyType();
        }
        mOccupants.Add(occ);
    }

    public void RemoveOccupant(D_ITargetable occ)
    {
        if(!mOccupants.Contains(occ))
        {
            Debug.LogWarning("Tried to remove " + occ.GetTransform().name + " from Occupants of node (" + x + "/" + y + ")");
            return;
        }

        mOccupants.Remove(occ);

        if (occ.GetOccupyType() >= mStatus)
        {
            mStatus = ENodeStatus.NS_Normal;
            foreach(D_ITargetable target in mOccupants)
            { 
                if (occ.GetOccupyType() > mStatus)
                {
                    mStatus = occ.GetOccupyType();
                }
            }
        }
    }
}
