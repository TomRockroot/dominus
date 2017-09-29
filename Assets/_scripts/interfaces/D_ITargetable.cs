using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface D_ITargetable
{
    Transform GetTransform();

    int GetParry();
    int GetIntegrity();
    void SetIntegrity(int integrity);

    void InteractPrimary(D_CharacterControl cntl);
    void InteractSecondary(D_CharacterControl cntl);

    void RegisterWithGameMaster();
    void UnregisterFromGameMaster();
}
