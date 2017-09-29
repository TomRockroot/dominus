using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_SkillWoodCutting : D_Skill
{

    public override bool ExecuteSkill(D_ITargetable target)
    {
        // Roll for hit (4)
        int roll = D_Dice.RollDie( mLevel ) + mOwner.GetAllEffectBoni(mBonusType) ;

        if (roll >= target.GetParry())
        {
            int dmg = D_Dice.RollDie(mOwner.GetAttributeDie(mAttribute)) + mOwner.GetAllEffectBoni(mAttribute);
            Debug.Log(name + " hit with " + roll + " for " + dmg + " Damage!");
            // is it a Raise?
            if (roll >= target.GetParry() + 4)
            {
                dmg += D_Dice.RollDie(D_Dice.EDieType.DT_D6);
                Debug.Log(name + " RAISE Damage to " + dmg);
            }

            // Reduce Integrity
            target.SetIntegrity(target.GetIntegrity() - dmg);
        }
        else
        {
            Debug.Log(name + " missed " + roll);
        }

        return true;
    }
}
