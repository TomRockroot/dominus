using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_URandom : D_AI_Utility
{
    public float mMin = 0f;
    public float mMax = 100f;

    public override float ComputePoints(D_ITargetable target, D_AIControl owner)
    {
        return Random.Range(mMin, mMax);
    }
}
