using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_AInteraction : D_AI_Action
{
    public D_Interaction mPreparedInteraction;

    public override D_AI_Action Test(D_ITargetable target, D_AIControl owner)
    {
        bool sane = false;

        List<D_Interaction> interactions = target.GetInteractions();

        foreach( D_Interaction interaction in interactions)
        {
            if(mPreparedInteraction.mSkillNeeded == interaction.mSkillNeeded)
            {
                sane = true;
                break;
            }
        }

        if (!sane)
        {
            return null;
        }
        else
        {
            return base.Test(target, owner);
        }
    }

    public override bool ExecuteAction()
    {
        if (mTarget == null)
        {
            return false;
        }

        Vector3 ownerToTarget = mTarget.GetTransform().position - mOwner.mCharacter.transform.position;

      //  Debug.Log("Executing " + name + " ownerToTarget " + ownerToTarget + " \n magnitude " + ownerToTarget.magnitude + " target " + mTarget.GetTransform().name);

        if(mOwner.mCharacter.mInteractionRange < ownerToTarget.magnitude)
        {
            // Move towards target
            mMoveVector = ownerToTarget;
            return true;
        }
        else
        {
            mTarget.Interact(mOwner, mPreparedInteraction);
            return false;
        }

    }
}
