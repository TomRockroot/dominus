using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "I_WoodCut", menuName = "Interaction/Wood Cut", order = 4)]
public class D_InteractionWoodCut : D_Interaction
{
    public override void ExecuteInteraction(D_Character subject, D_ITargetable target)
    {        
        if(D_GameMaster.GetInstance().GetCurrentController().mCharacter == subject)
        {
            D_UI_InteractionWheel.GetInstance().HideInteractions();
        }
        subject.StartCoroutine(WoodCutting(subject, target));
    }

    IEnumerator WoodCutting(D_Character subject, D_ITargetable target)
    {
        yield return subject.StartCoroutine(MoveToTarget(subject, target));

        subject.mAnimator.SetAnimation(mSkillNeeded);

        D_SoundMaster.GetInstance().PlaySound(ESoundCue.SC_WoodCut, -1);

        yield return new WaitForSeconds(subject.GetInteractionSpeed());

        int successes = 0;
        int diceRoll = D_Dice.RollDie(subject.GetSkillDie(mSkillNeeded)) + subject.GetEffectSkillBoni(mSkillNeeded);

        while (diceRoll > target.GetParry())
        {
            successes++;
            diceRoll -= 4;
        }

        string messageDebug = subject.name + " had " + successes + " successes while doing " + mName + "\non " + target.GetTransform().name + " with " + (target.GetIntegrity() - successes) + " integrity left!";
        string messageShort = successes + "! (" + (target.GetIntegrity() - successes) + " to go)";
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Interaction_Message)) Debug.Log(messageDebug);

        // TRIGGER THIS BY OBSERVER EVENT SYSTEM!
        D_UI_FloatTextCanvas.GetInstance().CreateFloatText(target.GetTransform().position, messageShort, EFloatText.FT_Success, messageDebug);
        

        if (target.SetIntegrity(target.GetIntegrity() - successes) > 0)
        {
            subject.StartCoroutine(WoodCutting(subject, target));
        }
        else
        {
            FinishInteraction(subject, target);
        }
    }

}
