using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class A_NodeManipulator : MonoBehaviour
{
    public ENodeManipulation mMouseLeft = ENodeManipulation.NM_Start;
    public ENodeManipulation mMouseRight = ENodeManipulation.NM_Goal;

    public void SetMouseStart(bool rightMouse)      {   SetMouseButton( ENodeManipulation.NM_Start,     rightMouse); }
    public void SetMouseGoal(bool rightMouse)       {   SetMouseButton(ENodeManipulation.NM_Goal,       rightMouse); }
    public void SetMouseFast(bool rightMouse)       {   SetMouseButton(ENodeManipulation.NM_Fast,       rightMouse); }
    public void SetMouseNormal(bool rightMouse)     {   SetMouseButton(ENodeManipulation.NM_Normal,     rightMouse); }
    public void SetMouseSlow(bool rightMouse)       {   SetMouseButton(ENodeManipulation.NM_Slow,       rightMouse); }
    public void SetMouseBlocked(bool rightMouse)    {   SetMouseButton(ENodeManipulation.NM_Blocked,    rightMouse); }

    public void SetMouseButton(ENodeManipulation manipulation, bool rightMouse)
    {
        if (rightMouse)
        {
            mMouseRight = manipulation;
        }
        else
        {
            mMouseLeft = manipulation;
        }
    }
}
