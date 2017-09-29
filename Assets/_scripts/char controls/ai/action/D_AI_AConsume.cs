﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_AConsume : D_AI_Action
{

    public override bool ExecuteAction()
    {
        if (mTarget == null)
        {
            return false;
        }

        Vector3 ownerToTarget = mTarget.GetTransform().position - mOwner.mCharacter.transform.position;

        Debug.Log("Executing " + name + " ownerToTarget " + ownerToTarget + " \n magnitude " + ownerToTarget.magnitude + " target " + mTarget.GetTransform().name);

        if(mOwner.mCharacter.mInteractionRange < ownerToTarget.magnitude)
        {
            // Move towards target
            mMoveVector = ownerToTarget;
            return true;
        }
        else
        {
            mTarget.InteractSecondary(mOwner);
            return false;
        }

    }
}