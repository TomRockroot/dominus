using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_AIControl : D_CharacterControlPath
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

    public override void OverrideMovement(bool val)
    {
        base.OverrideMovement(val);
        bDoing = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
        if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_AI_Message)) { Debug.Log("Initializing AIControl: " + name); }
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
            StartCoroutine(Think());
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
            yield return new WaitForEndOfFrame();
        }

        // Notice-Check
        foreach (D_ITargetable target in allTargets)
        {
            if(target as Object == mCharacter)
            {
                if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_AI_Message)) { Debug.Log("Ignore self!"); }
                continue;
            }
            if(target.Equals(null))
            {
                Debug.LogError("HARHARHARHAR! I AM EVIL CODE!");
            }
            if (target == null)
            {
                Debug.LogError("I do not understand this!");
            }

            if ((target.GetTransform().position - transform.position).magnitude < mAINoticeRange)
            {
                targetsInRange.Add(target);
            }
        }
        if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_AI_Message)) { Debug.Log(targetsInRange.Count + " targets in Range"); }

        D_AI_Action tempAction;

        // Then run action on each object
        foreach (D_AI_Action action in mViableActions)
        {
            foreach (D_ITargetable target in targetsInRange)
            {
                if (!target.Equals(null))
                {
                    tempAction = action.Test(target, this); // IF NOT VIABLE, RETURNS NULL
                    if (tempAction != null)
                    {
                        viableActionCopies.Add(tempAction);
                    }
                    else
                    {
                       // Debug.LogWarning(name + "'s Test was unviable for " + target.GetTransform().name);
                    }
                }
                else
                {
                   // Debug.LogWarning("WOLOLO!");
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

        if (D_GameMaster.GetInstance().IsFlagged(D_StructsAndEnums.EDebugLevel.DL_AI_Message)) { Debug.Log("I AM DONE THINKING! " + mTotalThinkCycles); }
        mTotalThinkCycles = 0;

        // ToDo: Get the best possible Action, and set it to be done!
        if (viableActionCopies.Count > 0)
        {
            foreach (D_AI_Action action in viableActionCopies)
            {
                if (mCurrentAction != null)
                {
                    // Compare points
                    if (mCurrentAction.mPoints < action.mPoints)
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
        }
        else
        {
            mCurrentAction = null;
        }
        
        

        bThinking = false;
    }
}
