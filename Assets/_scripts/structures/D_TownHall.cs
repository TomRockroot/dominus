using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_TownHall : D_Structure
{
    public D_Effect pPrimaryEffect;
    public D_Effect pSecondaryEffect;

    public override void Interact(D_CharacterControl cntl, D_Interaction interaction)
    {
        D_Character character = cntl.GetComponent<D_Character>();
        if (character == null) return;

        GameObject effectGO = Instantiate(pPrimaryEffect.gameObject, cntl.transform);
        character.mEffects.Add(effectGO.GetComponent<D_Effect>());
    }
}
