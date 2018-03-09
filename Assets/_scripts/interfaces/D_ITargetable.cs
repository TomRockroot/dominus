using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public interface D_ITargetable
{
    Transform GetTransform();

    string GetName();

    int GetParry();
    int GetIntegrity();
    int SetIntegrity(int integrity);

    void Interact(D_CharacterControl cntl, D_Interaction interaction);
    List<D_Interaction> GetInteractions();

    void ClearTargetedByInteraction();
    bool IsInteractionAllowed(D_CharacterControl cntl, EInteractionRestriction restriction);
    bool IsFlagged(EInteractionRestriction restriction);

    D_Maslow GetOnConsumptionMaslow();
    float GetInteractionRange();
    float GetInteractionSpeed();

    void RegisterWithGameMaster();
    void UnregisterFromGameMaster();
}
