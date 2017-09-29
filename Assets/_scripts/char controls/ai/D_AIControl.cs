using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_AIControl : D_CharacterControl
{
    public int mMaxThinkCyclesPerFrame = 3;
    public int mMaxThinkCyclesTotal = 100;

    public int mTotalThinkCycles = 0;

    public float mAINoticeRange = 20f;

    public bool bThinking;
    public bool bDoing;

    public Text mThinkBubbleUI;

    public D_AI_Action mCurrentAction;
    public List<D_AI_Action> mViableActions = new List<D_AI_Action>();
    public List<D_AI_Action> mWeightedActions = new List<D_AI_Action>();

    protected override void Initialize()
    {
        base.Initialize();
        Debug.Log("Initializing AIControl: " + name);
    }

    void Update ()
    {
		if(!bThinking && !bDoing)
        {
            StartCoroutine("Think");
        }
	}

    IEnumerator Think()
    {
        bThinking = true;

        if (mThinkBubbleUI != null)
        {
            mThinkBubbleUI.text = "?";
            yield return new WaitForSeconds(1.0f);
        }

        // Notice-Check
        List<D_ITargetable> allTargets = D_GameMaster.GetInstance().GetAllTargetables();
        List<D_ITargetable> targetsInRange = new List<D_ITargetable>();

        foreach(D_ITargetable target in allTargets)
        {
            if(target as Object == mCharacter)
            {
                Debug.Log("Ignore self!");
                continue;
            }
            if((target.GetTransform().position - transform.position).magnitude < mAINoticeRange)
            {
                targetsInRange.Add(target);
            }
        }
        Debug.Log(targetsInRange.Count + " targets in Range");

        while (bThinking)
        {
            // Then run action on each object
            foreach (D_AI_Action action in mViableActions)
            {
                foreach (D_ITargetable target in targetsInRange)
                {
                    action.Test(target);
                }
            }

            
            for (int i = 0; i < mMaxThinkCyclesPerFrame; i++)
            {
                
                if (mMaxThinkCyclesTotal > mTotalThinkCycles)
                {
                    // HERE IS WHERE THE MAGIC HAPPENS !!

                    // <(o.o<) ^(o.o)^ (>o.o)>
                   
                }
                else
                {
                    Debug.Log("DONE THINKING! \n TotalThinkCycles(" + mTotalThinkCycles + ") surpassed ViableActions(" + mViableActions.Count + ")");
                    bThinking = false;
                }
               
                mTotalThinkCycles++;
                
            }
            

            if (mThinkBubbleUI != null)
            {
                mThinkBubbleUI.text = "" + mTotalThinkCycles;
            }

            if (mTotalThinkCycles >= mMaxThinkCyclesTotal)
            {
                bThinking = false;
                Debug.Log("I AM DONE THINKING! " + mTotalThinkCycles);
                mTotalThinkCycles = 0;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

        
    }
}
