using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_InteractionWoodCut : D_Interaction
{
    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        mSubject = subject;
        mTarget = target;
        
        if(D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }
        StartCoroutine("WoodCutting");
    }

    IEnumerator WoodCutting()
    {
        mSubject.mController.OverrideMovement(true);

        Vector3 ownerToTarget = mTarget.GetTransform().position - mSubject.GetTransform().position;

       // Debug.Log("Owner To Target: " + ownerToTarget + " Owner: " + mSubject.GetTransform().name + " \nTarget: " + mTarget.GetTransform().name);

        while (ownerToTarget.magnitude > mSubject.GetInteractionRange() )
        {
            // IF BIGGER THEN ONE -> NORMALIZE ??
            
            mSubject.mController.mMoveVector = Vector3.Scale(ownerToTarget,  new Vector3(1f, 0f, 1f)).normalized;
            yield return new WaitForEndOfFrame();

            ownerToTarget = mTarget.GetTransform().position - mSubject.GetTransform().position;

           // Debug.Log("Owner To Target: " + ownerToTarget + " Owner: " + mSubject.GetTransform().position + " \nTarget: " + mTarget.GetTransform().position);
        }

        mSubject.mController.mMoveVector = Vector3.zero;
        // SET PLAYERCONTROL CLICK DESTINATION
        (D_GameMaster.GetInstance().GetCurrentController() as D_PlayerControlClick).SetDestination(mSubject.GetTransform().position);
        mSubject.mAnimator.SetAnimation(mSkillNeeded);

        yield return new WaitForSeconds(mSubject.GetInteractionSpeed());

        int successes = 0;
        int diceRoll = D_Dice.RollDie(mSubject.GetSkillDie(mSkillNeeded)) + mSubject.GetEffectSkillBoni(mSkillNeeded);

        while (diceRoll > mTarget.GetParry())
        {
            successes++;
            diceRoll -= 4;
        }

        string messageDebug = mSubject.name + " had " + successes + " successes while doing " + mName + "\non " + mTarget.GetTransform().name + " with " + (mTarget.GetIntegrity() - successes) + " integrity left!";
        string messageShort = successes + "! (" + (mTarget.GetIntegrity() - successes) + " to go)";
        Debug.Log(messageDebug);

        // TRIGGER THIS BY OBSERVER EVENT SYSTEM!
        D_UI_FloatTextCanvas.GetInstance().CreateFloatText(mTarget.GetTransform().position, messageShort, EFloatText.FT_Success, messageDebug);
        mTarget.SetIntegrity(mTarget.GetIntegrity() - successes);

        mSubject.mController.OverrideMovement(false);

        Destroy(gameObject);
    }

}
