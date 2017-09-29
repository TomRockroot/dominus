using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_UI_Needs : MonoBehaviour
{
    public float mOffset = -50f;

    public Image mWaterCountUI;
    public Image mFoodCountUI;
    public Image mSleepCountUI;
    public Image mWarmthCountUI;
    public Image mSexCountUI;

    public void UpdateUI(D_StructsAndEnums.ENeed needType, float value)
    {
        switch(needType)
        {
            case D_StructsAndEnums.ENeed.N_Water:
                mWaterCountUI.rectTransform.anchoredPosition = new Vector2(mOffset + value, mWaterCountUI.rectTransform.anchoredPosition.y);
                break;

            case D_StructsAndEnums.ENeed.N_Food:
                mFoodCountUI.rectTransform.anchoredPosition = new Vector3(mOffset + value, mFoodCountUI.rectTransform.anchoredPosition.y);
                break;

            case D_StructsAndEnums.ENeed.N_Sleep:
                mSleepCountUI.rectTransform.anchoredPosition = new Vector3(mOffset + value, mSleepCountUI.rectTransform.anchoredPosition.y);
                break;

            case D_StructsAndEnums.ENeed.N_Warmth:
                mWarmthCountUI.rectTransform.anchoredPosition = new Vector3(mOffset + value, mWarmthCountUI.rectTransform.anchoredPosition.y);
                break;

            case D_StructsAndEnums.ENeed.N_Sex:
                mSexCountUI.rectTransform.anchoredPosition = new Vector3(mOffset + value, mSexCountUI.rectTransform.anchoredPosition.y);
                break;
        }
    }
}
