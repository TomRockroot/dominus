using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "I_OpenUI", menuName = "Interaction/Open UI", order = 5)]
public class D_InteractionOpenUI : D_Interaction
{
    public EUserInterface mWindow = EUserInterface.UI_None;

    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Message)) Debug.Log("Interaction(" + name + "): Subject: " + subject.name + " Target: " + target.GetTransform().name);
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }

        subject.StartCoroutine(OpenUI(subject, target));
    }

    IEnumerator OpenUI(D_Character subject, D_ITargetable target)
    {
        yield return subject.StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation("OpenUI");

        D_UI_Manager.GetInstance().OpenWindow(mWindow, target);
        
        FinishInteraction(subject, target);
    }
}