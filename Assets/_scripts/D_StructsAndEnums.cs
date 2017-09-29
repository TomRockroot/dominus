using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class D_StructsAndEnums
{
	public enum EFaction
    {
        F_Player,
        F_Wildlife,       
    }

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

