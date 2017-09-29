using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_ItemConsumable : D_Item
{
    public D_Effect mEffect;

    // ===== PICK UP =====
    public override void InteractPrimary(D_CharacterControl cntl)
    {
        // if character has skill
        D_Skill skill = cntl.mCharacter.GetSkill(D_StructsAndEnums.EBonus.B_PickItem);
        if (skill != null)
        {
            skill.ExecuteSkill(this);
        }
        else
        {
            Debug.Log("No Skill for " + D_StructsAndEnums.EBonus.B_PickItem);
        }
    }

    // ===== CONSUME =====
    public override void InteractSecondary(D_CharacterControl cntl)
    {
        D_Skill skill = cntl.mCharacter.GetSkill(D_StructsAndEnums.EBonus.B_ConsumeItem);
        if (skill != null)
        {
            if(skill.ExecuteSkill(this))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("No Skill for " + D_StructsAndEnums.EBonus.B_ConsumeItem);
        }
    }
}
