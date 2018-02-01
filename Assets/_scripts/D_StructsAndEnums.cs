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

    [System.Flags]
    public enum EInteractionRestriction
    {
        IR_World = (1 << 0),
        IR_Inventory = (1 << 1)
    }

    public enum EEffectType
    {
        ET_Skill,
        ET_Attribute,
        ET_Derived,
        ET_Maslow
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

    public enum EMaslow
    {
        // Physiological (Tribal)
            // Food
        M_Sweet,
        M_Fruity,
        M_Ribald,
        M_Fine,
        M_Hydrated,

        M_Sleep,
        M_Housing,

        // Safety and Security
        M_FriendsFamily,
        M_Health,
        M_Employment,
        M_Property,

        // Love and Belonging
        M_Romance,
        M_Company,
        M_Friendship,
        M_Fellowship,

        // Self-esteem
        M_Confidence,   // Increasing of skills
        M_Achievement,  // Luxury of possession 
        M_Respect,      // Respect of others (Guidance, Leadership, Paragon, Mentoring)
        M_Awe,          // Respect by others 

        // Self-actualization
        M_Morality,         // Law and Philosophy
        M_Creativity,       // Art (Architecture, sculpture, music, painting, writing) 
        M_ProblemSolving,   // Science
        M_Entertainment     // Spontanity
    }

    public enum ESoundCue
    {
        SC_ClickUI,
        SC_WoodCut
    }

    [System.Flags]
    public enum EDebugLevel
    {
        DL_General_Message  = (1 << 0),
        DL_General_Warning  = (1 << 1),
        DL_General_Error    = (1 << 2),

        DL_AI_Message       = (1 << 3),
        DL_AI_Warning       = (1 << 4),
        DL_AI_Error         = (1 << 5),

        DL_Sound_Message    = (1 << 6),
        DL_Sound_Warning    = (1 << 7),
        DL_Sound_Error      = (1 << 8),

        DL_Interaction_Message  = (1 << 9),
        DL_Interaction_Warning  = (1 << 10),
        DL_Interaction_Error    = (1 << 11),

        DL_Item_Message = (1 << 12),
        DL_Item_Warning = (1 << 13),
        DL_Item_Error   = (1 << 14),

        DL_Structure_Message = (1 << 15),
        DL_Structure_Warning = (1 << 16),
        DL_Structure_Error   = (1 << 17),

        DL_Character_Message = (1 << 18),
        DL_Character_Warning = (1 << 19),
        DL_Character_Error   = (1 << 20),

        DL_UI_Message = (1 << 21),
        DL_UI_Warning = (1 << 22),
        DL_UI_Error   = (1 << 23)
    }

    public enum EUserInterface
    {
        UI_Culture,
        UI_Inventory,
        UI_Equipment,
        UI_Maslow,

        UI_Craft,
        UI_Statistics,
        UI_Dialog,
        UI_Menu,

        UI_None
    }

    public enum ECameraMode
    {
        CM_Follow,
        CM_Focus,
        CM_Freefly,
        CM_Rail
    }

    public enum ECraftSlot
    {
        CS_SlotOne,
        CS_SlotTwo,

        CS_SlotResult
    }

    public enum ENodeManipulation
    {
        NM_Start,
        NM_Goal,
        NM_Fast,
        NM_Normal,
        NM_Slow,
        NM_Blocked
    }

    public enum ENodeStatus
    {
        NS_Normal,
        NS_Slow,
        NS_Fast,
        NS_Occupied,
        NS_Blocked
    }
}

