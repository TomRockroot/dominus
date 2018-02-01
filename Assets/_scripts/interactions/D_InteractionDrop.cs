using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_InteractionDrop : D_Interaction {

    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }

        StartCoroutine(Drop(subject, target));
    }

    IEnumerator Drop(D_Character subject, D_ITargetable target)
    {
        yield return StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation("Drop Item");

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        // Do Shit
        if(target is D_Item)
        {
            D_Item item = target as D_Item;

            item.SwitchSelfInventory(item.mStashedInInventory, D_GameMaster.GetInstance());
            item.Hide(false);
            item.transform.position = subject.transform.position;
            item.ClearFlags();
            item.SetFlag(EInteractionRestriction.IR_World);
        }
        else
        {
            if(D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Warning) ) Debug.LogWarning("Tried to Drop something else as 'D_Item' " + target.GetTransform().name);
        }

        Debug.Log("LOL");
        FinishInteraction(subject, target);
    }
}
