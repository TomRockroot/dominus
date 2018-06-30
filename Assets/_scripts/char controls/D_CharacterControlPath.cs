﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CharacterControlPath : D_CharacterControl
{
    Coroutine mFollowCoroutine;

    public void WalkPath(A_Waypoint goal)
    {
        // MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this class are only used to reference these coroutines, and do not hold any exposed properties or functions.
        if (mFollowCoroutine != null)
        {
            Debug.LogError("Möp!");
            StopAllCoroutines();
        }
        mFollowCoroutine = StartCoroutine(FollowWaypoints(goal));
    }

    IEnumerator FollowWaypoints(A_Waypoint goal)
    {
        A_Grid grid = goal.mNode.mGrid;
        Terrain terrain = grid.mTerrain;
        Vector3 nodeLocalPos = new Vector3(goal.mNode.x * grid.mNodeDistance, terrain.SampleHeight(terrain.transform.position + new Vector3(goal.mNode.x * grid.mNodeDistance, 0, goal.mNode.y * grid.mNodeDistance)), goal.mNode.y * grid.mNodeDistance);
        Vector3 nodeWorldPos = nodeLocalPos + terrain.transform.position;

        if (goal.mVia != null)
        {
            yield return StartCoroutine(FollowWaypoints(goal.mVia));
        }
        else
        {
            transform.position = nodeWorldPos;
            yield break;
        }

        while ((nodeWorldPos - transform.position).magnitude > 0.1f)
        {
            transform.position += (nodeWorldPos - transform.position).normalized * Time.deltaTime * mCharacter.GetPace();
            yield return new WaitForEndOfFrame();
        }
    }
}
