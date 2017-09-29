using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_Utility : MonoBehaviour
{
    public D_AI_Sensor mSensor;

    public virtual float ComputePoints(D_ITargetable target, D_AIControl owner)
    {
        return 0.0f;
    }
}
