using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using D_StructsAndEnums;

public class D_Character : MonoBehaviour, D_IEffectable, D_IInventory, D_ITargetable, IPointerClickHandler
{
    // Other
    public float mInteractionRange = 1.0f;
    public float mInteractionSpeed = 1.0f;

    public D_Interaction mTargetedByInteraction;

    public D_CharacterControl mController;
    public D_CharacterAnimator mAnimator;

    // Character Dice
    //  public D_Dice.EDieType[] mCharacterDice = new D_Dice.EDieType[ Enum.GetValues( typeof(D_StructsAndEnums.ECharacterDice)).Length ];
    protected Dictionary<ESkillDice, EDieType> mSkillDict = new Dictionary<ESkillDice, EDieType>();
    protected Dictionary<EAttributeDice, EDieType> mAttributeDict = new Dictionary<EAttributeDice, EDieType>();

    // Attributes 
    public EDieType GetAttributeDie(EAttributeDice attribute)
    {
        EDieType value;
        if( mAttributeDict.TryGetValue(attribute, out value) ) // Can be improved -->
        {
            return value;
        }

        return EDieType.DT_None;
    }

    private void PopulateAttributeDictionary()
    {
        foreach(EAttributeDice attributeDie in Enum.GetValues(typeof(EAttributeDice)))
        {
            mAttributeDict.Add(attributeDie, EDieType.DT_D4);
        }

        Debug.Log("Populated AttributeDictionary for " + name + " with " + mAttributeDict.Count + " Attributes!");
    }

    // Skills
    public EDieType GetSkillDie(ESkillDice skill)
    {
        EDieType value;
        if( mSkillDict.TryGetValue(skill, out value) )
        {
            return value;
        }

        return EDieType.DT_None;
    }

    private void PopulateSkillDictionary()
    {
        foreach (ESkillDice skillDie in Enum.GetValues(typeof(ESkillDice)))
        {
            mSkillDict.Add(skillDie, EDieType.DT_D4);
        }

        Debug.Log("Populated SkillDictionary for " + name + " with " + mSkillDict.Count + " Skills!");
    }

    // Derived 
    public int GetPace()
    {
        return Mathf.FloorToInt( D_Dice.DieTypeToInt(GetAttributeDie(EAttributeDice.AD_Agility) ) + GetEffectDerivedBoni(EDerivedStat.DS_Pace));
    }

    public int GetParry()
    {
        return Mathf.FloorToInt( D_Dice.DieTypeToInt(GetAttributeDie(EAttributeDice.AD_Agility) ) * 0.5f + 2 + GetEffectDerivedBoni(EDerivedStat.DS_Parry));
    }

    public int GetToughness()
    {
        return Mathf.FloorToInt(D_Dice.DieTypeToInt(GetAttributeDie(EAttributeDice.AD_Vigor) ) * 0.5f + 2 + GetEffectDerivedBoni(EDerivedStat.DS_Toughness));
    }

    public int GetCharisma()
    {
        return Mathf.FloorToInt(D_Dice.DieTypeToInt(GetAttributeDie(EAttributeDice.AD_Vigor) ) * 0.5f + 2 + GetEffectDerivedBoni(EDerivedStat.DS_Charisma));
    }

    // ============ Needs ============
    

    // ============ Equipment ============
    public List<D_Equipment> mEquipment = new List<D_Equipment>();

    // ============ Inventory ============
    public List<D_Item> mInventory = new List<D_Item>();

    public void AddToInventory(D_Item item)
    {
        mInventory.Add(item);

        if(D_UI_CharacterSheet.GetInstance().mInventoryUI != null)
        {
            D_UI_CharacterSheet.GetInstance().mInventoryUI.UpdateUI(mInventory);
        }
    }

    public void RemoveFromInventory(D_Item item)
    {
        mInventory.Remove(item);

        item.GetTransform().parent = transform.parent;
    }

    public void DropInventory()
    {
        foreach (D_Item item in mInventory)
        {
            item.GetTransform().parent = transform.parent;
        }
    }

    // Effects (incorporates Needs)
    public float mHappiness;

    public List<D_Effect> mEffects = new List<D_Effect>();
    public List<D_Effect> GetEffects() { return mEffects; }

    public int GetEffectAttributeBoni(EAttributeDice attribute)
    {
        int sum = 0;

        foreach (D_Effect effect in mEffects)
        {
            sum += effect.GetPassiveAttribute(attribute);
        }

        return sum;
    }

    public int GetEffectSkillBoni(ESkillDice skill)
    {
        int sum = 0;

        foreach(D_Effect effect in mEffects)
        {
            sum += effect.GetPassiveSkill(skill);
        }

        return sum;
    }

    public int GetEffectDerivedBoni(EDerivedStat derived)
    {
        int sum = 0;

        foreach (D_Effect effect in mEffects)
        {
            sum += effect.GetPassiveDerived(derived);
        }

        return sum;
    }

    void Start()
    {
        RegisterWithGameMaster();
        PopulateAttributeDictionary();
        PopulateSkillDictionary();

        mAnimator = GetComponent<D_CharacterAnimator>();
        mController = GetComponent<D_CharacterControl>();
    }

    void Update()
    {
        foreach(D_Effect effect in mEffects)
        {
            effect.RunEffect(this);
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
            if (possibleInt == interaction)
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

        mTargetedByInteraction = Instantiate(interaction);
        mTargetedByInteraction.transform.SetParent(cntl.transform);

        mTargetedByInteraction.ExecuteInteraction(cntl.mCharacter, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        D_GameMaster.GetInstance().GetCurrentController().PrepareTarget(this);
    }

    public float GetInteractionRange()
    {
        return mInteractionRange;
    }

    public float GetInteractionSpeed()
    {
        return mInteractionSpeed;
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
