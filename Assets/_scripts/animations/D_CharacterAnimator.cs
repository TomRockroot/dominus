using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_CharacterAnimator : MonoBehaviour {

	public void SetAnimation(ESkillDice skill)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) { Debug.Log(name + " is animating " + skill + " (by ESkillDice)"); }
    }

    public void SetAnimation(string animationName)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) { Debug.Log(name + " is animating " + animationName + " (by animationName)"); }
    }
}
