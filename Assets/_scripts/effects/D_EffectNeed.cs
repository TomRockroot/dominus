using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_EffectNeed : D_Effect
{
    public D_StructsAndEnums.ENeed mNeedType;

    public override void RunEffect(D_IEffectable target)
    {
        base.RunEffect(target);

        target.SatisfyNeed(mNeedType, (mAmount / mStartLifeTime) * Time.deltaTime );
    }
	
}
