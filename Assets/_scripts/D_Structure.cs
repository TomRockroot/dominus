using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Structure : MonoBehaviour, D_ITargetable
{
    public int mParry     = 2;
    public int mIntegrity = 100;

    // === Get / Set by Interface ==
    public Transform GetTransform() { return transform; }

    public int GetParry() { return mParry; }
    public int GetIntegrity() { return mIntegrity; }
    public virtual void SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity < 0)
        {
            Destroy(gameObject);
        }
    }

    public D_StructsAndEnums.EFaction mFaction;

    void Start()
    {
        RegisterWithGameMaster();
    }

    void OnDestroy()
    {
        UnregisterFromGameMaster();
    }


    public List<D_Interaction> mPossibleInteractions;
    public List<D_Interaction> GetInteractions() { return mPossibleInteractions; }

    public void Interact(D_CharacterControl cntl, D_Interaction interaction)
    {
        D_Interaction foundInteraction = null;
        foreach (D_Interaction possibleInt in mPossibleInteractions)
        {
            if (possibleInt.mSkillNeeded == interaction.mSkillNeeded)
            {
                foundInteraction = possibleInt;
                break;
            }
        }

        if (foundInteraction == null)
        {
            Debug.LogError("Moppelkotze!");
            return;
        }

        // if character has skill
        D_Skill skill = cntl.mCharacter.GetSkill(foundInteraction.mSkillNeeded);
        if (skill != null)
        {
            skill.ExecuteSkill(this);
        }
        else
        {
            Debug.Log("No Skill for " + foundInteraction.mSkillNeeded);
        }
    }

    public void RegisterWithGameMaster()
    {
        D_GameMaster.GetInstance().RegisterTargetable(this);
    }

    public void UnregisterFromGameMaster()
    {
        D_GameMaster.GetInstance().UnregisterTargetable(this);
    }
}
