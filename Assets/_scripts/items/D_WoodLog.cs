using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_WoodLog : D_Item
{
    public override void Interact(D_CharacterControl cntl, D_Interaction interaction)
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
}
