using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_ULerp : D_AI_Utility
{
    public float mPointsMin = 0f;
    public float mPointsMax = 100f;

    public override float ComputePoints(D_ITargetable target, D_AIControl owner)
    {
        float value = mSensor.RunSensor(target, owner, out mOutMin, out mOutMax);
        return Map(0f, owner.mAINoticeRange, mPointsMax, mPointsMin, value );
    }


    /*This basically uses the "rule of three" (Dreisatz) to map a value
	 * f.e. colours have to be between 0 and 1, but you have the values 3.1415(a1) and 42(a2) and want to know, 
	 *  where between 0(b1) and 1(b2) your 39(s) would be located. Hope that helps.
	 */
    public float Map(float a1, float a2, float b1, float b2, float s)
    {
        return (b1 + ((s - a1) * (b2 - b1)) / (a2 - a1));
    }

}
