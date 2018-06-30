using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using D_StructsAndEnums;

public class A_Indicator : MonoBehaviour,  IPointerEnterHandler, IPointerDownHandler
{
    private A_NodeManipulator mManipulator;
   // private A_Pathfinder mPathfinder;
    public  A_Node mNode;

    public Color mStartColor = Color.blue;
    public Color mGoalColor  = Color.magenta;
    public Color mFastColor  = Color.green;
    public Color mNormalColor= Color.white;
    public Color mSlowColor  = Color.yellow;
    public Color mBlockedColor = Color.red;

    public void OnPointerDown(PointerEventData eventData)
    {
        ENodeManipulation manipulation;

        Debug.Log(" Click! (" + mNode.x + "/" + mNode.y + ")");

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            manipulation = mManipulator.mMouseLeft;
        }
        else
        {
            manipulation = mManipulator.mMouseRight;
        }

        SetMode(manipulation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(" Enter! (" + mNode.x + "/" + mNode.y + ") M1: " + Input.GetMouseButton(0) + " M2: " + Input.GetMouseButton(1));

        ENodeManipulation manipulation;
        if (Input.GetMouseButton(0))
        {
            manipulation = mManipulator.mMouseLeft;
            SetMode(manipulation);
        }
        else if(Input.GetMouseButton(1)) 
        {
            manipulation = mManipulator.mMouseRight;
            SetMode(manipulation);
        }
    }

    public void SetMode(ENodeManipulation manipulation)
    {
        Debug.Log("Set Node (" + mNode.x +"/" + mNode.y + ") from " + mNode.mStatus + " to " + manipulation);

        switch( manipulation )
        {
            case ENodeManipulation.NM_Start:
                GetComponent<Renderer>().material.color = mStartColor;
            //    mPathfinder.SetStartNode(mNode);

                break;

            case ENodeManipulation.NM_Goal:
                GetComponent<Renderer>().material.color = mGoalColor;
             //   mPathfinder.SetGoalNode(mNode);

                break;

            case ENodeManipulation.NM_Fast:
                GetComponent<Renderer>().material.color = mFastColor;
                mNode.mStatus = ENodeStatus.NS_Fast;
                break;

            case ENodeManipulation.NM_Normal:
                GetComponent<Renderer>().material.color = mNormalColor;
                mNode.mStatus = ENodeStatus.NS_Normal;
                break;

            case ENodeManipulation.NM_Slow:
                GetComponent<Renderer>().material.color = mSlowColor;
                mNode.mStatus = ENodeStatus.NS_Slow;
                break;

            case ENodeManipulation.NM_Blocked:
                GetComponent<Renderer>().material.color = mBlockedColor;
                mNode.mStatus = ENodeStatus.NS_Blocked;
                break;
        }

        
    }
   
    void Start ()
    {
        mManipulator = mNode.mGrid.mTerrain.GetComponent<A_NodeManipulator>();
     //   mPathfinder  = mNode.mGrid.mTerrain.GetComponent<A_Pathfinder>();
        SetMode(ENodeManipulation.NM_Normal);
	}
}
