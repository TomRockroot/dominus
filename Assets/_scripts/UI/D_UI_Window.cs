using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_UI_Window : MonoBehaviour
{
    public virtual void Open(D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_UI_Warning)) Debug.LogWarning("No Open()-override for " + name);
        D_SoundMaster.GetInstance().PlayError();
    }

    public virtual void Close(D_ITargetable target)
    {
        if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_UI_Warning)) Debug.LogWarning("No Close()-override for " + name);
    }
}
