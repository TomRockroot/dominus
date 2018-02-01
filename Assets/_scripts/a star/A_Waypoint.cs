using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class A_Waypoint
{
    public A_Node     mNode;
    public A_Waypoint mVia;

    float mDistance;

    public float mStatusMod = 1f;
    public float mCost;

    public float mCombined;
	
    public A_Waypoint(A_Node node, A_Waypoint via, float distance, bool diagonal  )
    {
        mNode = node;
        mVia  = via;
        mDistance = distance;

        switch(node.mStatus)
        {
            case ENodeStatus.NS_Normal:
                mStatusMod = node.mGrid.mNodeDistance * 1f;
                break;

            case ENodeStatus.NS_Slow:
                mStatusMod = node.mGrid.mNodeDistance * 2f;
                break;

            case ENodeStatus.NS_Fast:
                mStatusMod = node.mGrid.mNodeDistance * 0.5f;
                break;

            case ENodeStatus.NS_Occupied:
                mStatusMod = node.mGrid.mNodeDistance * 10f;
                break;

            case ENodeStatus.NS_Blocked:
                mStatusMod = Mathf.Infinity;
                break;
        }

        if (mVia != null)
        {
            mCost = mVia.mCost + (diagonal ? 1.4f : 1f) * mStatusMod; // * node.mStatus ;
        }
        else
        {
            mCost = 0f;
        }

        mCombined = mCost + mDistance;
    }
}
