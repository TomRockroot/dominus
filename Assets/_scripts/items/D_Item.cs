using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_Item : MonoBehaviour, D_ITargetable
{
    public D_IInventory mStashedInInventory;
    public Sprite mInventorySprite;

    public int mParry = 2;
    public int mIntegrity = 100;

    // === Get / Set by Interface ==
    public Transform GetTransform() { return transform; }

    public int GetParry() { return mParry; }
    public int GetIntegrity() { return mIntegrity; }
    public void SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity < 0)
        {
            Destroy(gameObject);
        }
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
            // <(o.o<) ^(o.o)^ (>o.o)>
            // ToDo: CHANGE THIS TO foundInteraction.ExecuteInteraction(this) !!!!  !!!!! !!!!! !!!! !!!!!!! !!!!!!!! !!!!!!!! !!!!!!! !!!!!!! !!!!!!!!!!
            // =================================== HERE! =======================================
            skill.ExecuteSkill(this);
        }
        else
        {
            Debug.Log("No Skill for " + foundInteraction.mSkillNeeded);
        }
    }


    public void RemoveFromInventory(D_IInventory from)
    {
        if (from != null)
        {
          //  Debug.Log("from: " + from + " removed!");
            from.RemoveFromInventory(this);
            mStashedInInventory = null;
        }
    }

    public void AddToInventory(D_IInventory to)
    {
        mStashedInInventory = to;
        to.AddToInventory(this);
    }

    public void SwitchInventory(D_IInventory from, D_IInventory to)
    {
       // Debug.Log("from: " + from + " to: " + to);
        RemoveFromInventory(from);
        AddToInventory(to);
    }

    void Start()
    {
        RegisterWithGameMaster();
    }

    void OnDestroy()
    {
        RemoveFromInventory(mStashedInInventory);
        UnregisterFromGameMaster();
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
