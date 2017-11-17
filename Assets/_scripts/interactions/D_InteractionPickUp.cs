using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_InteractionPickUp : D_Interaction {

    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        if(target is D_Item)
        {
            D_Item item = (target as D_Item);
            item.SwitchSelfInventory((target as D_Item).mStashedInInventory, subject);
            item.GetComponent<MeshRenderer>().enabled = false;
        }

        D_UI_FloatTextCanvas.GetInstance().CreateFloatText(subject.GetTransform().position, "Picked up " + target.GetTransform().name, EFloatText.FT_Speech);
        D_UI_InteractionWheel.GetInstance().HideInteractions();

        Destroy(gameObject);
    }
}
