using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerControl : D_CharacterControl
{
    protected D_ITargetable mPreparedTarget;
    

    protected override void Initialize()
    {
        D_GameMaster.GetInstance().SetCurrentController(this);

        base.Initialize();
    }

    public D_ITargetable GetPreparedTarget()
    {
        return mPreparedTarget;
    }

    public void PrepareTarget(D_ITargetable target)
    {
        D_UI_InteractionWheel.GetInstance().HideInteractions();

        mPreparedTarget = target;

        D_UI_InteractionWheel.GetInstance().ShowInteractions(target);
    }
}
