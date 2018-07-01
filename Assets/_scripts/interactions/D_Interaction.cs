using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_Interaction : ScriptableObject
{
    public string mName = "Some Interaction";

    public ESkillDice mSkillNeeded = ESkillDice.SD_None;
    public D_Effect mEffect;

    public bool bImplemented = true;

    [HideInInspector]
    public EInteractionRestriction mRestrictionFlags;

    public virtual void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Message)) Debug.Log(subject.GetTransform().name + " is doing '" + mName + "' with " + target.GetTransform().name);

        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }
    }

    protected void FinishInteraction(D_Character subject, D_ITargetable target)
    {
        subject.mController.OverrideMovement(false);
        target.ClearTargetedByInteraction();
    }

    protected IEnumerator MoveToTarget(D_Character subject, D_ITargetable target)
    {
        if ((D_GameMaster.GetInstance().GetCurrentController() as D_PlayerControlPath) != null)
        {
            Debug.Log("Interaction: MoveToTarget (Path)");

            yield return subject.StartCoroutine((D_GameMaster.GetInstance().GetCurrentController() as D_PlayerControlPath).FindPathToCoroutine(subject.GetNode().GetGrid(), target.GetTransform().position));
        }
        else
        {
            Debug.Log("Interaction: MoveToTarget (Classic)");

            subject.mController.OverrideMovement(true);
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Message)) Debug.Log("MoveToTarget: " + subject.mName + " is moving to target " + target.GetTransform().name + "!");
            Vector3 ownerToTarget = Vector3.Scale(target.GetTransform().position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(subject.GetTransform().position, new Vector3(1f, 0f, 1f));
            Vector3 moveVector;

            while (ownerToTarget.magnitude > subject.GetInteractionRange())
            {
                moveVector = Vector3.Scale(ownerToTarget, new Vector3(1f, 0f, 1f));
                if (moveVector.magnitude > 1.0f)
                {
                    moveVector.Normalize();
                }

                subject.mController.mMoveVector = moveVector;
                yield return new WaitForEndOfFrame();

                if (target.Equals(null))
                {
                    if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Error)) Debug.LogError("Interaction: MoveToTarget - Lost Target!");
                    FinishInteraction(subject, target);
                    yield break;
                }
                ownerToTarget = Vector3.Scale(target.GetTransform().position, new Vector3(1f, 0f, 1f)) - Vector3.Scale(subject.GetTransform().position, new Vector3(1f, 0f, 1f));
            }

            subject.mController.mMoveVector = Vector3.zero;
        }

        if (subject.mController == D_GameMaster.GetInstance().GetCurrentController())
        {
            if (subject.mController as D_PlayerControlClick != null)
            {
                (D_GameMaster.GetInstance().GetCurrentController() as D_PlayerControlClick).SetDestination(subject.GetTransform().position);
            }
        }
        else
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Error)) Debug.LogError("MoveToTarget: " + subject.mName + " has the wrong controller: " + subject.mController);
        }
    }
}
