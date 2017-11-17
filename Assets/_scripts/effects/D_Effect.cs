using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_Effect : MonoBehaviour
{
    public EEffectType mEffectType;

    public ESkillDice       mSkillType;
    public EAttributeDice   mAttributeType;
    public EDerivedStat     mDerivedType;

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

    public int GetPassiveSkill(ESkillDice skillType)
    {
        if (skillType != mSkillType)
        {
            return 0;
        }
        else
        {
            return mAmount;
        }
    }

    public int GetPassiveAttribute(EAttributeDice attributeType)
    {
        if (attributeType != mAttributeType)
        {
            return 0;
        }
        else
        {
            return mAmount;
        }
    }

    public int GetPassiveDerived(EDerivedStat derivedType)
    {
        if (derivedType != mDerivedType)
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
