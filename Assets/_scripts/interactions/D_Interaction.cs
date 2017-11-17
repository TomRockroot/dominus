using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_Interaction : MonoBehaviour
{
    public string mName = "Some Interaction";

    public ESkillDice mSkillNeeded = ESkillDice.SD_None;
    public D_Effect mEffect;

    protected D_Character mSubject;
    protected D_ITargetable mTarget;

    public virtual void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {
        Debug.Log(subject.GetTransform().name + " is doing '" + mName + "' with " + target.GetTransform().name);

    }
}
