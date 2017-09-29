using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_Action : MonoBehaviour
{
    public D_AI_Utility mConnectedUtility;
    public float mPoints;

    public D_ITargetable mTarget;
    public D_AIControl mOwner;

    public Vector3 mMoveVector = Vector3.zero;

    public D_AI_Action Test(D_ITargetable target, D_AIControl owner)
    {
        D_AI_Action copy = Instantiate(this);
        if (copy.mConnectedUtility != null)
        {
            copy.mPoints = copy.mConnectedUtility.ComputePoints(target);
        }
        copy.mTarget = target;
        copy.mOwner  = owner;

        copy.transform.parent = owner.transform;

        return copy;
    }

    /** @return true, as long as it is running smoothly
        @return false, when done or disturbed

        maybe change that to enum E_AI_ActionRunningThingy
    */
    public virtual bool ExecuteAction()
    {
        return false;
    }
}
