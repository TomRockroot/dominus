using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_InteractionOpenCrafting : D_Interaction
{
    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_Interaction_Message)) Debug.Log("Interaction(" + name + "): Subject: " + subject.name + " Target: " + target.GetTransform().name);
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }

        StartCoroutine(OpenCrafting(subject, target));
    }

    IEnumerator OpenCrafting(D_Character subject, D_ITargetable target)
    {
        yield return StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation("OpenCrafting");

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        //Do Shit
        if (target is D_ICrafter)
        {
            if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_Interaction_Message)) Debug.Log("Interaction(" + name + "): WINNING! I AM D_ICrafter!");

            D_UI_Manager.GetInstance().OpenWindow(D_StructsAndEnums.EUserInterface.UI_Craft, target);
        }
        else
        {
            if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_Interaction_Error)) Debug.LogError("Interaction(" + name + "): " + target.GetTransform().name + " was not D_ICrafter!");
        }

        FinishInteraction(subject, target);
    }
}