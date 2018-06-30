using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "I_Dominate", menuName = "Interaction/Dominate", order = 1)]
public class D_InteractionDominate : D_Interaction
{
    
    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }

        subject.StartCoroutine(Dominate(subject, target));
    }
    
    IEnumerator Dominate(D_Character subject, D_ITargetable target)
    {
        yield return subject.StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation(mSkillNeeded);

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        // Do stuff

        FinishInteraction(subject, target);
    }
}

