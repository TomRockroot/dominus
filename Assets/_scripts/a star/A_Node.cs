using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class A_Node
{
    public A_Grid mGrid;
    public ENodeStatus mStatus = ENodeStatus.NS_Normal;
    public A_Indicator mIndicator;

    public int x;
    public int y;

    public A_Node(int _x, int _y, A_Grid grid)
    {
        x = _x;
        y = _y;

        mGrid = grid;
    }
}
