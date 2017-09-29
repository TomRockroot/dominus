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
        if(! (item is D_ItemConsumable))
        {
            return false;
        }
        D_ItemConsumable consumable = item as D_ItemConsumable;
        
        if(consumable.mEffect == null)
        {
            Debug.LogWarning("No Effect in " + consumable.name);
            return false;
        }

        // if(mOwner possess the right need)
        if(consumable.mEffect is D_EffectNeed)
        {
            if(mOwner.HasRelevantNeed( ((D_EffectNeed) consumable.mEffect).mNeedType ))
            {
                // Spawn Effect
                GameObject effectGO = Instantiate(consumable.mEffect.gameObject, mOwner.transform);
                mOwner.mEffects.Add(effectGO.GetComponent<D_Effect>());

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            GameObject effectGO = Instantiate(consumable.mEffect.gameObject, mOwner.transform);
            mOwner.mEffects.Add(effectGO.GetComponent<D_Effect>());

            return true;
        }
    }
}
