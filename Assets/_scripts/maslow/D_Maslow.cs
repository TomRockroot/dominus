using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_Maslow : MonoBehaviour
{
    public string mName;
    public Sprite mIcon;

    public EMaslow mCategory;
	public float mHappiness;

    public float mLifetimeMax;
    public float mLifetimeCurrent;

    public float GetHappiness(D_Character character, bool simulate = false)
    {
        if(simulate)
        {
            return mHappiness + character.GetEffectMaslowBoni(mCategory);
        }

        if (mLifetimeCurrent > mLifetimeMax * 0.5f)
        {
            return mHappiness + character.GetEffectMaslowBoni(mCategory);
        }

        if(mLifetimeCurrent > mLifetimeMax * 0.25f)
        {
            return (mHappiness + character.GetEffectMaslowBoni(mCategory)) * 0.5f;
        }

        return (mHappiness + character.GetEffectMaslowBoni(mCategory)) * 0.25f;
    }

    void Start()
    {
        mLifetimeCurrent = mLifetimeMax;
    }

    void Update()
    {
        mLifetimeCurrent -= Time.deltaTime;
        if(mLifetimeCurrent < 0f)
        {
            FinishMaslow();
        }
    }

    public void FinishMaslow()
    {
        Destroy(gameObject);
    }
}
