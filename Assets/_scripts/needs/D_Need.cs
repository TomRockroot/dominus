using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Need : MonoBehaviour {

    public D_StructsAndEnums.ENeed mNeedType;

    public float mSatisfaction;

    protected D_Character mOwner;

    public void SetOwner(D_Character owner)
    {
        mOwner = owner;
    }

    public void SetSatisfaction(float value)
    {
        mSatisfaction = value;
        if (D_UI_CharacterSheet.GetInstance().mNeedsUI != null)
        {
            D_UI_CharacterSheet.GetInstance().mNeedsUI.UpdateUI(mNeedType, value);
        }
    }

    public virtual void CheckSatisfaction()
    {
        if (mSatisfaction > 0)
        {
            SetSatisfaction(mSatisfaction - Time.deltaTime);
        }
    }
}
