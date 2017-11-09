using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class D_Character : MonoBehaviour, D_IEffectable, D_IInventory, D_ITargetable
{
    // Other
    public float mInteractionRange = 1.0f;

    // Character Dice
  //  public D_Dice.EDieType[] mCharacterDice = new D_Dice.EDieType[ Enum.GetValues( typeof(D_StructsAndEnums.ECharacterDice)).Length ];

    // Attributes
    public D_Dice.EDieType mStrength = D_Dice.EDieType.DT_D4;
    public D_Dice.EDieType mAgility  = D_Dice.EDieType.DT_D4;
    public D_Dice.EDieType mSpirit   = D_Dice.EDieType.DT_D4;
    public D_Dice.EDieType mSmarts   = D_Dice.EDieType.DT_D4;
    public D_Dice.EDieType mVigor    = D_Dice.EDieType.DT_D4;

    public D_Dice.EDieType GetAttributeDie(D_StructsAndEnums.EBonus attribute)
    {
        switch(attribute)
        {
            case D_StructsAndEnums.EBonus.B_Strength:
                return mStrength;
            case D_StructsAndEnums.EBonus.B_Agility:
                return mAgility;
            case D_StructsAndEnums.EBonus.B_Spirit:
                return mSpirit;
            case D_StructsAndEnums.EBonus.B_Smarts:
                return mSmarts;
            case D_StructsAndEnums.EBonus.B_Vigor:
                return mVigor;
        }

        return D_Dice.EDieType.DT_None;
    }

    // Derived 
    public int GetPace()
    {
        return Mathf.FloorToInt(D_Dice.DieTypeToInt(mAgility) + GetAllEffectBoni(D_StructsAndEnums.EBonus.B_Pace));
    }

    public int GetParry()
    {
        return Mathf.FloorToInt(D_Dice.DieTypeToInt(mAgility) * 0.5f + 2 + GetAllEffectBoni(D_StructsAndEnums.EBonus.B_Parry));
    }

    public int GetToughness()
    {
        return Mathf.FloorToInt(D_Dice.DieTypeToInt(mVigor) * 0.5f + 2 + GetAllEffectBoni(D_StructsAndEnums.EBonus.B_Toughness));
    }

    public int GetCharisma()
    {
        return Mathf.FloorToInt(D_Dice.DieTypeToInt(mVigor) * 0.5f + 2 + GetAllEffectBoni(D_StructsAndEnums.EBonus.B_Charisma));
    }

    // Skills
    public List<D_Skill> mSkills = new List<D_Skill>();
    public D_Skill GetSkill(D_StructsAndEnums.EBonus bonus)
    {
        foreach (D_Skill skill in mSkills)
        {
            if(skill.mBonusType == bonus)
            {
                return skill;
            }
        }
        return null;
    }

    // ============  Needs ============
    public List<D_Need> pNeeds = new List<D_Need>();
        // Needs must be instantiated (or values will be changed on the prefab!!! )
    public List<D_Need> mNeeds = new List<D_Need>();

    public float mHappiness;
    public float mFatigue;

    public bool HasRelevantNeed(D_StructsAndEnums.ENeed needType)
    {
        foreach (D_Need need in mNeeds)
        {
            if (need.mNeedType == needType)
            {
                return true;
            }
        }

        Debug.LogWarning("NoNeed " + needType);
        return false;
    }

    public bool SatisfyNeed(D_StructsAndEnums.ENeed needType, float amount)
    {
        foreach (D_Need need in mNeeds)
        {
            if (need.mNeedType == needType)
            {
                need.SetSatisfaction( need.mSatisfaction + amount);
                return true;
            }
        }

        Debug.LogWarning("NoNeed " + needType);
        return false;
    }

    // ============ Equipment ============
    public List<D_Equipment> mEquipment = new List<D_Equipment>();

    // ============ Inventory ============
    public List<D_Item> mInventory = new List<D_Item>();

    public void AddToInventory(D_Item item)
    {
        Debug.Log("MOPPELKOTZE");
        mInventory.Add(item);
        if(D_UI_CharacterSheet.GetInstance().mInventoryUI != null)
        {
            D_UI_CharacterSheet.GetInstance().mInventoryUI.UpdateUI(mInventory);
        }
    }

    public void RemoveFromInventory(D_Item item)
    {
        bool what = mInventory.Remove(item);
        Debug.Log(what + " the hell?");
        item.GetTransform().parent = transform.parent;
    }

    public void DropInventory()
    {
        foreach (D_Item item in mInventory)
        {
            item.GetTransform().parent = transform.parent;
        }
    }

    // Effects 
    public List<D_Effect> mEffects = new List<D_Effect>();
    public List<D_Effect> GetEffects() { return mEffects; }  


    public int GetAllEffectBoni(D_StructsAndEnums.EBonus bonus)
    {
        int sum = 0;

        foreach(D_Effect effect in mEffects)
        {
            sum += effect.GetPassiveBonus(bonus);
        }

        return sum;
    }

    // Virtues

    // Vices

    void Start()
    {
        RegisterWithGameMaster();

        /*
        Debug.Log(name + " has " + mCharacterDice.Length + " CharacterDice!");
        Debug.Log(D_StructsAndEnums.ECharacterDice.CD_Agility       + ": " + mCharacterDice[(int)D_StructsAndEnums.ECharacterDice.CD_Agility]);
        Debug.Log(D_StructsAndEnums.ECharacterDice.CD_Strength      + ": " + mCharacterDice[(int)D_StructsAndEnums.ECharacterDice.CD_Strength]);
        Debug.Log(D_StructsAndEnums.ECharacterDice.CD_Smarts        + ": " + mCharacterDice[(int)D_StructsAndEnums.ECharacterDice.CD_Smarts]);
        Debug.Log(D_StructsAndEnums.ECharacterDice.CD_Spirit        + ": " + mCharacterDice[(int)D_StructsAndEnums.ECharacterDice.CD_Spirit]);
        Debug.Log(D_StructsAndEnums.ECharacterDice.CD_Vigor         + ": " + mCharacterDice[(int)D_StructsAndEnums.ECharacterDice.CD_Vigor]);
        Debug.Log(D_StructsAndEnums.ECharacterDice.CD_WoodCutting   + ": " + mCharacterDice[(int)D_StructsAndEnums.ECharacterDice.CD_WoodCutting]);
        */

        foreach (D_Need need in pNeeds)
        {
            GameObject needGO = Instantiate(need.gameObject, this.transform);
            mNeeds.Add(needGO.GetComponent<D_Need>());
            needGO.GetComponent<D_Need>().SetOwner(this);
        }

        foreach(D_Skill skill in mSkills)
        {
            skill.SetOwner(this);
        }
    }

    void Update()
    {
        foreach(D_Effect effect in mEffects)
        {
            effect.RunEffect(this);
        }

        foreach(D_Need need in mNeeds)
        {
            need.CheckSatisfaction();
        }
    }

    void OnDestroy()
    {
        UnregisterFromGameMaster();
    }


    // ===== INTERACTION !! =====
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

    public Transform GetTransform() { return transform; }
    public int GetIntegrity() { return 0; }
    public void SetIntegrity(int integrity) { }

    public void RegisterWithGameMaster()
    {
        D_GameMaster.GetInstance().RegisterTargetable(this);
    }

    public void UnregisterFromGameMaster()
    {
        D_GameMaster.GetInstance().UnregisterTargetable(this);
    }
}
