using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "I_Consume", menuName = "Interaction/Consume", order = 1 )]
public class D_InteractionConsume : D_Interaction
{
    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }

        subject.StartCoroutine(Consume(subject, target));
    }

    IEnumerator Consume(D_Character subject, D_ITargetable target)
    {
        yield return subject.StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation(mSkillNeeded);

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        if (target.Equals(null))
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Error)) { Debug.LogError("Target for " + mName + " disappeared mid-Corourtine!"); }
            FinishInteraction(subject, target);
            yield break;
        }

        if (target.GetOnConsumptionMaslow() == null)
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Error)) Debug.LogError(mName + " could not resolve - no Maslow!");
        }
        else
        {
            D_Maslow maslow = Instantiate(target.GetOnConsumptionMaslow(), subject.transform);
            maslow.transform.localPosition = Vector3.zero;
            if(subject.SetMaslow(maslow))
            {
                target.SetIntegrity(-1);
            }
            else
            {
                Destroy(maslow.gameObject);
            }
        }


        FinishInteraction(subject, target);
    }
}