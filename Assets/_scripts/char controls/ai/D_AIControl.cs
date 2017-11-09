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

    public override Vector3 GetMoveVector()
    {
        if(!bDoing)
        {
            return Vector3.zero;
        }
        else
        {
            return mCurrentAction.mMoveVector.normalized;
        }
    }

    new protected void Update ()
    {
        if(mCurrentAction != null)
        {
            bDoing = mCurrentAction.ExecuteAction();
        }
        else
        {
            bDoing = false;
        }

		if(!bThinking && !bDoing)
        {
            StartCoroutine("Think");
        }

        base.Update();
	}

    IEnumerator Think()
    {
        bThinking = true;

        if (mCurrentAction != null)
        {
            Destroy(mCurrentAction.gameObject);
        }

        List<D_AI_Action> viableActionCopies = new List<D_AI_Action>();
        List<D_ITargetable> allTargets = D_GameMaster.GetInstance().GetAllTargetables();
        List<D_ITargetable> targetsInRange = new List<D_ITargetable>();
        int thinkCycles = 0;

        if (mThinkBubbleUI != null)
        {
            mThinkBubbleUI.text = "?";
            yield return new WaitForSeconds(0.001f);
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

        D_AI_Action tempAction;

        // Then run action on each object
        foreach (D_AI_Action action in mViableActions)
        {
            foreach (D_ITargetable target in targetsInRange)
            {
                tempAction = action.Test(target, this);
                if (tempAction != null)
                {
                    viableActionCopies.Add(tempAction);
                }
                thinkCycles++;
                mTotalThinkCycles++;

                if(mThinkBubbleUI != null)
                {
                    mThinkBubbleUI.text = "" + mTotalThinkCycles;
                }

                if (mTotalThinkCycles >= mMaxThinkCyclesTotal)
                {
                    break;
                }

                if (thinkCycles >= mMaxThinkCyclesPerFrame)
                {
                    thinkCycles = 0;
                    yield return new WaitForEndOfFrame();
                }
            }

            if (mTotalThinkCycles >= mMaxThinkCyclesTotal)
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
                // Compare points
                if(mCurrentAction.mPoints < action.mPoints)
                {
                    Destroy(mCurrentAction.gameObject);
                    mCurrentAction = action;
                }
                else
                {
                    Destroy(action.gameObject);
                }
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
