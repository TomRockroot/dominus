using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "I_PickUp", menuName = "Interaction/Pick Up", order = 3)]
public class D_InteractionPickUp : D_Interaction {

    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }
        subject.StartCoroutine(PickUp(subject, target));
    }

    IEnumerator PickUp(D_Character subject, D_ITargetable target)
    {
        yield return subject.StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation("Pick Item");

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        if (target is D_Item)
        {
            D_Item item = (target as D_Item);
            item.SwitchSelfInventory(item.GetStashedInInventory(), subject);
            item.ClearFlags();
            item.SetFlag(EInteractionRestriction.IR_Inventory);

            D_UI_FloatTextCanvas.GetInstance().CreateFloatText(subject.GetTransform().position, "Picked up " + target.GetTransform().name, EFloatText.FT_Speech);
        }

        FinishInteraction(subject, target);
    }
}
