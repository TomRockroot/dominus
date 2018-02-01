using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_AInteraction : D_AI_Action
{
    public Transform InspectorTarget;

    public D_Interaction mPreparedInteraction;

    public override D_AI_Action Test(D_ITargetable target, D_AIControl owner)
    {
        if(target.Equals(null))
        {
            Debug.LogError("Why do I even Test something null? Where is this coming from?");
            return null;
        }
        bool sane = false;
        

        // OMG SPAGGEDDI HERE I COME
        if(!target.IsInteractionAllowed(owner, mPreparedInteraction.mRestrictionFlags))
        {
            if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_AI_Warning)) { Debug.LogWarning("AI: " + owner.name + " is not allowed to interact with " + target.GetTransform().name ); }
            return null;
        }


        List<D_Interaction> interactions = target.GetInteractions();

        foreach ( D_Interaction interaction in interactions)
        {
            if (mPreparedInteraction == interaction)
            {
                sane = true;
                break;
            }
        }

        if (!sane)
        {
            // I DO RETURN NULL MYSELF AFTER TEST
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
        if (mTarget.Equals(null))
        {
          //  Debug.LogError("OMG?!");
            return false;
        }
        InspectorTarget = mTarget.GetTransform();

        Vector3 ownerToTarget = Vector3.Scale(mTarget.GetTransform().position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(mOwner.mCharacter.transform.position, new Vector3(1f, 0f, 1f));
        // Vector3 ownerToTarget = mTarget.GetTransform().position - mOwner.mCharacter.transform.position;

        if (mTarget.IsInteractionAllowed(mOwner, mPreparedInteraction.mRestrictionFlags))
        {
            if (mOwner.mCharacter.mInteractionRange < ownerToTarget.magnitude)
            {
                // Move towards target
                //  Debug.Log(mOwner.name + " is moving towards " + mTarget.GetTransform().name);
                mMoveVector = ownerToTarget;
                return true;
            }
            else
            {
                if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_AI_Message)) { Debug.Log(mOwner.name + " is interacting with " + mTarget.GetTransform().name); }

                mTarget.Interact(mOwner, mPreparedInteraction);
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
