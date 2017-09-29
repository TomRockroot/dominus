using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class D_Skill : MonoBehaviour
{
    public D_StructsAndEnums.EBonus mAttribute;
    public D_StructsAndEnums.EBonus mBonusType;

    public D_Dice.EDieType mLevel = D_Dice.EDieType.DT_D4;

    protected D_ITargetable     mTarget;
    protected D_Character       mOwner;

    public void SetTarget(D_ITargetable target)
    {
        mTarget = target;
    }

    public void SetOwner(D_Character owner)
    {
        mOwner = owner;
    }

    public virtual bool ExecuteSkill(D_ITargetable target)
    {
        Debug.LogWarning("MOPPELKOTZE (>o.o)>");
        return true;
    }
}
