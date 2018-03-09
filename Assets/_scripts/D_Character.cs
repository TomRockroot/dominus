using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using D_StructsAndEnums;

public class D_Character : MonoBehaviour, D_IEffectable, D_IInventory, D_ITargetable, D_ICrafter, IPointerClickHandler
{
    public string mName = "Some Guy";

    // Other
    public float mInteractionRange = 1.0f;
    public float mInteractionSpeed = 1.0f;

    public int mIntegrity = 3;

    public D_Interaction mTargetedByInteraction;
    [HideInInspector]
    public EInteractionRestriction mRestrictionFlags = EInteractionRestriction.IR_World;

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

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) { Debug.Log("Populated AttributeDictionary for " + name + " with " + mAttributeDict.Count + " Attributes!"); }
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

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) { Debug.Log("Populated SkillDictionary for " + name + " with " + mSkillDict.Count + " Skills!"); }
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
    protected Dictionary<EMaslow, D_Maslow> mMaslowDict = new Dictionary<EMaslow, D_Maslow>();
    public List<D_Maslow> mAllMaslows = new List<D_Maslow>();

    public D_Maslow GetMaslow(EMaslow key)
    {
        D_Maslow value;
        if(mMaslowDict.TryGetValue(key, out value))
        {
            return value;
        }

        return null;
    }

    public bool SetMaslow(D_Maslow maslowNew)
    {
        D_Maslow maslowOld = GetMaslow(maslowNew.mCategory);
        if(maslowOld == null)
        {
            mMaslowDict.Remove(maslowNew.mCategory);
            mMaslowDict.Add(maslowNew.mCategory, maslowNew);
            mAllMaslows.Add(maslowNew);
            return true;
        }

        if (maslowOld.GetHappiness(this) > maslowNew.GetHappiness(this, true))
        {
            return false;
        }

        mAllMaslows.Add(maslowOld);
        maslowOld.FinishMaslow();

        mMaslowDict.Remove(maslowNew.mCategory);
        mMaslowDict.Add(maslowNew.mCategory, maslowNew);
        mAllMaslows.Add(maslowNew);
        return true;
    }

    // ============ Equipment ============
    public List<D_Equipment> mEquipment = new List<D_Equipment>();

    // ============ Inventory ============
    protected List<D_Item> mInventory = new List<D_Item>();

    public List<D_Item> GetInventory()
    {
        return mInventory;
    }

    public void AddToInventory(D_Item item)
    {
        mInventory.Add(item);

        if(this == (D_Character)D_UI_Manager.GetInstance().mInventoryUI.mCurrentPossessor)
        {
            D_UI_Manager.GetInstance().mInventoryUI.AddItem(item);
        }
    }

    public void RemoveFromInventory(D_Item item)
    {
        mInventory.Remove(item);

        item.GetTransform().parent = transform.parent;

        if (D_UI_Manager.GetInstance() == null) return;

        if (this == (D_Character) D_UI_Manager.GetInstance().mInventoryUI.mCurrentPossessor)
        {
            D_UI_Manager.GetInstance().mInventoryUI.RemoveItem(item);
        }
    }

    public void DropInventory()
    {
        foreach (D_Item item in mInventory)
        {
            item.GetTransform().parent = transform.parent;
        }
    }

    public void HideInventory()
    {
        if (this == D_GameMaster.GetInstance().GetCurrentController().mCharacter)
        {
            D_UI_Manager.GetInstance().OpenWindow(EUserInterface.UI_None, this);
        }

        foreach(D_Item item in mInventory)
        {
            item.Hide(true);
        }
    }

    // Recipes
    public List<D_Recipe> mKnownRecipes = new List<D_Recipe>();

    public List<D_Recipe> GetRecipes()
    {
        return mKnownRecipes;
    }

    public List<D_Item> GetSimilarItems(D_Item blueprint)
    {
        List<D_Item> collection = new List<D_Item>();

        foreach(D_Item item in GetInventory())
        {
            if(item.GetName() == blueprint.GetName())
            {
                collection.Add(item);
                Debug.Log(" == " + blueprint.GetName() + " == WHOOP == " + item.GetName() + " == ");
            }
            else
            {
                Debug.Log(" vv " + blueprint.GetName() + " vv DERP vv " + item.GetName() + " vv ");
            }
        }

        return collection;
    }

    public void Craft(D_Item result, D_Item first, D_Item second)
    {

    }

    public D_IInventory AsInventory() { return this; }

    // Effects
    public float mHappiness;

    public List<D_Effect> mEffects = new List<D_Effect>();
    public List<D_Effect> GetEffects() { return mEffects; }

    public int GetEffectAttributeBoni(EAttributeDice attribute)
    {
        int sum = 0;

        foreach (D_Effect effect in mEffects)
        {
            if (effect.mEffectType == EEffectType.ET_Attribute)
            {
                sum += effect.GetPassiveAttribute(attribute);
            }
        }

        return sum;
    }

    public int GetEffectSkillBoni(ESkillDice skill)
    {
        int sum = 0;

        foreach(D_Effect effect in mEffects)
        {
            if (effect.mEffectType == EEffectType.ET_Skill)
            {
                sum += effect.GetPassiveSkill(skill);
            }
        }

        return sum;
    }

    public int GetEffectDerivedBoni(EDerivedStat derived)
    {
        int sum = 0;

        foreach (D_Effect effect in mEffects)
        {
            if (effect.mEffectType == EEffectType.ET_Derived)
            {
                sum += effect.GetPassiveDerived(derived);
            }
        }

        return sum;
    }

    public int GetEffectMaslowBoni(EMaslow maslow)
    {
        int sum = 0;

        foreach (D_Effect effect in mEffects)
        {
            if (effect.mEffectType == EEffectType.ET_Derived)
            {
                sum += effect.GetPassiveMaslow(maslow);
            }
        }

        return sum;
    }

    public string GetName()
    {
        return mName;
    }

    void Start()
    {
        RegisterWithGameMaster();
        PopulateAttributeDictionary();
        PopulateSkillDictionary();
        HideInventory();

        mAnimator = GetComponent<D_CharacterAnimator>();
        mController = GetComponent<D_CharacterControl>();
    }

    void Update()
    {
        foreach (D_Effect effect in mEffects)
        {
            effect.RunEffect(this);
        }


        // Happiness Shit (Maslow etc)
        float happiness = 0f;
        bool foundMissing = false;
        foreach (D_Maslow maslow in mAllMaslows)
        {
            if (maslow != null)
            {
                happiness += maslow.GetHappiness(this);
            }
            else
            {
                foundMissing = true;
            }
        }
        if(foundMissing)
        {
            mAllMaslows.RemoveAll(D_Maslow => D_Maslow == null);
        }
        mHappiness = happiness;

        if (this == D_GameMaster.GetInstance().GetCurrentController().mCharacter)
        {
            D_UI_Manager.GetInstance().mMaslowUI.SetHappiness(mHappiness);
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

        mTargetedByInteraction = interaction;

        mTargetedByInteraction.ExecuteInteraction(cntl.mCharacter, this);
    }

    public void ClearTargetedByInteraction()
    {
        mTargetedByInteraction = null;
    }

    public bool IsInteractionAllowed(D_CharacterControl cntl, EInteractionRestriction restriction)
    {
        return IsFlagged(restriction);
    }

    public bool IsFlagged(EInteractionRestriction flag)
    {
        return (mRestrictionFlags & flag) == flag;
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

    public D_Maslow GetOnConsumptionMaslow() { return null; }
    public Transform GetTransform() { return transform; }
    public int GetIntegrity() { return mIntegrity; }
    public int SetIntegrity(int integrity)
    {
        mIntegrity = integrity;
        if (mIntegrity < 0)
        {
            Destroy(gameObject);
        }
        return mIntegrity;
    }

    public void RegisterWithGameMaster()
    {
        D_GameMaster.GetInstance().RegisterTargetable(this);
    }

    public void UnregisterFromGameMaster()
    {
        if(D_GameMaster.GetInstance() != null)
            D_GameMaster.GetInstance().UnregisterTargetable(this);
    }
}
