using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_InteractionPickUp : D_Interaction {

    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }
        StartCoroutine(PickUp(subject, target));
    }

    IEnumerator PickUp(D_Character subject, D_ITargetable target)
    {
        yield return StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation("Pick Item");

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        if (target is D_Item)
        {
            D_Item item = (target as D_Item);
            item.SwitchSelfInventory(item.mStashedInInventory, subject);
            item.ClearFlags();
            item.SetFlag(EInteractionRestriction.IR_Inventory);

            D_UI_FloatTextCanvas.GetInstance().CreateFloatText(subject.GetTransform().position, "Picked up " + target.GetTransform().name, EFloatText.FT_Speech);
        }

        FinishInteraction(subject, target);
    }
}
