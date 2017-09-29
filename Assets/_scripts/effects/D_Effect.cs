using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Effect : MonoBehaviour {

    public D_StructsAndEnums.EBonus mBonusType = D_StructsAndEnums.EBonus.B_None;
    public int mAmount;

    public float mStartLifeTime = 1f;
    protected float mLifeTime;
    public D_IEffectable mTarget;

    protected bool bInitialized = false;

    public virtual void RunEffect(D_IEffectable target)
    {
        if(!bInitialized)
        {
            bInitialized = InitizializeEffect(target);
        }

        mLifeTime -= Time.deltaTime;
        if(mLifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    protected bool InitizializeEffect(D_IEffectable target)
    {
        if (mTarget == null)
        {
            mTarget = target;
        }

        mLifeTime = mStartLifeTime;

        return true;
    }

    public int GetPassiveBonus(D_StructsAndEnums.EBonus bonusType)
    {
        if (bonusType != mBonusType)
        {
            return 0;
        }
        else
        {
            return mAmount;
        }
    }

    void OnDestroy()
    {
        mTarget.GetEffects().Remove(this);
    }
}
