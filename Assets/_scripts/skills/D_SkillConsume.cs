using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_SkillConsume : D_Skill
{

    public override bool ExecuteSkill(D_ITargetable target)
    {
        // if( is actually a consumable)
        D_Item item = target.GetTransform().GetComponent<D_Item>();

        if(item == null)
        {
            return false;
        }

        D_Interaction foundInteraction = null;

        foreach (D_Interaction possibleInt in item.mPossibleInteractions)
        {
            if (possibleInt.mSkillNeeded == D_StructsAndEnums.EBonus.B_ConsumeItem)
            {
                foundInteraction = possibleInt;
                break;
            }
        }

        if(foundInteraction == null)
        {
            if(foundInteraction.mEffect == null)
            {
                Debug.LogWarning("No Effect in " + item.name);
                return false;
            }
            Debug.LogWarning("No Interaction in " + item.name);
            return false;
        }

        // if(mOwner possess the right need)
        if(foundInteraction.mEffect is D_EffectNeed)
        {
            if(mOwner.HasRelevantNeed( ((D_EffectNeed) foundInteraction.mEffect).mNeedType ))
            {
                // Spawn Effect
                GameObject effectGO = Instantiate(foundInteraction.mEffect.gameObject, mOwner.transform);
                mOwner.mEffects.Add(effectGO.GetComponent<D_Effect>());
                Destroy(item.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            GameObject effectGO = Instantiate(foundInteraction.mEffect.gameObject, mOwner.transform);
            mOwner.mEffects.Add(effectGO.GetComponent<D_Effect>());
            Destroy(item.gameObject);
            return true;
        }
    }
}
