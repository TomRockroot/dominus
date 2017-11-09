﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface D_ITargetable
{
    Transform GetTransform();

    int GetParry();
    int GetIntegrity();
    void SetIntegrity(int integrity);

    void Interact(D_CharacterControl cntl, D_Interaction interaction);
    List<D_Interaction> GetInteractions();
  
    void RegisterWithGameMaster();
    void UnregisterFromGameMaster();
}
