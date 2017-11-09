using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Interaction : MonoBehaviour
{
    public D_StructsAndEnums.EBonus mSkillNeeded;
    public D_Effect mEffect;

    public virtual bool ExecuteInteraction()
    {
        return true;
    }
}
