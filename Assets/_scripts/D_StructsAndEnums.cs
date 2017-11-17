using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace D_StructsAndEnums
{
	public enum EFaction
    {
        F_Player,
        F_Wildlife,       
    }

    public enum EDieType
    {
        DT_D4,
        DT_D6,
        DT_D8,
        DT_D10,
        DT_D12,
        DT_None
    }


    // deprecate this
    public enum EBonus
    {
        // Attributes
        B_Strength,
        B_Agility,
        B_Spirit,
        B_Smarts,
        B_Vigor,
        
        // Derived
        B_Pace,
        B_Parry,
        B_Toughness,
        B_Charisma,

        // Skill
        B_WoodCutting,
        B_PickItem,
        B_ConsumeItem,

        // Other
        B_SatisfyNeed,
        
        // None
        B_None
    }

    public enum EEffectType
    {
        ET_Skill,
        ET_Attribute,
        ET_Derived
    }

    public enum ESkillDice
    {
        SD_None,

        // Skills
        SD_WoodCutting,
        SD_Cooking,
        SD_Smithing,
        SD_Hunting,

        SD_Fighting,
        SD_Shooting
    }

    public enum EAttributeDice
    {
        // Attributes
        AD_Strength,
        AD_Agility,
        AD_Spirit,
        AD_Smarts,
        AD_Vigor
    }

    public enum EDerivedStat
    {
        DS_Pace,
        DS_Parry,
        DS_Toughness,
        DS_Charisma
    }

    public enum EFloatText
    {
        FT_Debug,
        FT_Success,
        FT_Speech
    }

    public enum ENeed
    {
        // Physiological
        N_Food,
        N_Water,
        N_Sex,
        N_Sleep,
        N_Warmth

        /*
        // Safety
        N_Body,
        N_Employment,
        N_Resources,
        N_Morality,
        N_SafetyFamily,
        N_Health,
        N_Property,

        // Love / Belonging
        N_Friendship,
        N_Family,
        N_Inimacy,

        // Esteem
        N_SelfEsteem,
        N_Confidence,
        N_Achievement,
        N_Respect

        // Self-actualization
        */
    }
}

