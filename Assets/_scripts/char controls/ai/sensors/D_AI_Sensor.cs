using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_AI_Sensor : MonoBehaviour
{
    public virtual float RunSensor(D_ITargetable target, D_AIControl owner, out float min, out float max)
    {
        min = 0f;
        max = 0f;
        return 0f;
    }
	
}
