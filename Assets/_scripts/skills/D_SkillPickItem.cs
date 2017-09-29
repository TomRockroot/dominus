using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_SkillPickItem : D_Skill {

    public override bool ExecuteSkill(D_ITargetable target)
    {
        D_Item item = target.GetTransform().GetComponent<D_Item>();

        if (item != null)
        {
            if (item.mStashedInInventory != null)
            {
                item.SwitchInventory(item.mStashedInInventory, mOwner);
            }
            else
            {
                Debug.LogWarning(name + " was missing Stashed Inventory!");
                item.AddToInventory(mOwner);
            }
            target.GetTransform().parent = mOwner.transform;
            return true;
        }
        Debug.LogError("Something went horribly wrong with " + name);
        return false;
    }
}
