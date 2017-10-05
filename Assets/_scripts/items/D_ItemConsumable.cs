using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_ItemConsumable : D_Item
{
    public D_Interaction mInteraction;

    // ===== PICK UP =====
    public override void Interact(D_CharacterControl cntl, D_Interaction interaction)
    {
        // if character has skill
        D_Skill skill = cntl.mCharacter.GetSkill(mInteraction.mSkillNeeded);
        if (skill != null)
        {
            skill.ExecuteSkill(this);
        }
        else
        {
            Debug.Log("No Skill for " + mInteraction.mSkillNeeded);
        }
    }
}
