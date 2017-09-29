using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface D_IEffectable
{
    List<D_Effect> GetEffects();
    bool SatisfyNeed(D_StructsAndEnums.ENeed needType, float amount);
}
