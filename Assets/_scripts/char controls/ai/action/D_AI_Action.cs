using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_Action : MonoBehaviour
{
    public D_AI_Utility mConnectedUtility;
    public float mPoints;

    public D_ITargetable mTarget;

    public D_AI_Action Test(D_ITargetable target)
    {
        D_AI_Action copy = Instantiate(this);
        copy.mPoints = copy.mConnectedUtility.ComputePoints(target);
        copy.mTarget = target;
        return copy;
    }

    /** @return true, as long as it is running smoothly
        @return false, when done or disturbed

        maybe change that to enum E_AI_ActionRunningThingy
    */
    public bool ExecuteAction(D_Character owner)
    {
        return false;
    }
}
