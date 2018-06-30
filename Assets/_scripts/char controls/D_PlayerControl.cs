using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_PlayerControl : D_CharacterControlPath
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

    public void PrepareTarget(D_ITargetable target, EInteractionRestriction restriction = EInteractionRestriction.IR_World)
    {
        D_UI_InteractionWheel.GetInstance().HideInteractions();

        mPreparedTarget = target;

        if(D_UI_InteractionWheel.GetInstance().ShowInteractions(target, restriction))
        {
            Camera.main.GetComponent<D_CameraFollow>().SetCameraState(ECameraMode.CM_Focus, target.GetTransform());
        }
    }
}
