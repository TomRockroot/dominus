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
        if(mCurrentAction != null)
        {
            bDoing = mCurrentAction.ExecuteAction(mCharacter);
        }
        else
        {
            bDoing = false;
        }

		if(!bThinking && !bDoing)
        {
            StartCoroutine("Think");
        }
	}

    IEnumerator Think()
    {
        bThinking = true;

        List<D_AI_Action> viableActionCopies = new List<D_AI_Action>();
        List<D_ITargetable> allTargets = D_GameMaster.GetInstance().GetAllTargetables();
        List<D_ITargetable> targetsInRange = new List<D_ITargetable>();
        int thinkCycles = 0;

        if (mThinkBubbleUI != null)
        {
            mThinkBubbleUI.text = "?";
            yield return new WaitForSeconds(1.0f);
        }

        // Notice-Check
        foreach (D_ITargetable target in allTargets)
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

        
        // Then run action on each object
        foreach (D_AI_Action action in mViableActions)
        {
            foreach (D_ITargetable target in targetsInRange)
            {
                viableActionCopies.Add( action.Test(target) );
                thinkCycles++;
                mTotalThinkCycles++;

                if(mThinkBubbleUI != null)
                {
                    mThinkBubbleUI.text = "" + mTotalThinkCycles;
                }

                if (mTotalThinkCycles >= mMaxThinkCyclesPerFrame)
                {
                    break;
                }

                if (thinkCycles >= mMaxThinkCyclesPerFrame)
                {
                    thinkCycles = 0;
                    yield return new WaitForEndOfFrame();
                }
            }

            if (mTotalThinkCycles >= mMaxThinkCyclesPerFrame)
            {
                
                break;
            }
        }

        Debug.Log("I AM DONE THINKING! " + mTotalThinkCycles);
        mTotalThinkCycles = 0;

        // ToDo: Get the best possible Action, and set it to be done!
        
        foreach(D_AI_Action action in viableActionCopies)
        {
            if(mCurrentAction != null)
            {

            }
            else
            {
                mCurrentAction = action;
            }
            bDoing = true;
        }
        


        bThinking = false;
    }
}
