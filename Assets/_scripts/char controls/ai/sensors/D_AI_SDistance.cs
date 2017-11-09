using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_SDistance : D_AI_Sensor
{
    public override float RunSensor(D_ITargetable target, D_AIControl owner, out float min, out float max)
    {
        min = 0f;
        max = owner.mAINoticeRange;
        return (target.GetTransform().position - owner.transform.position).magnitude;
    }

}
