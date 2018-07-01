using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class A_Pathfinder
{
    public static IEnumerator TryPath(A_Grid grid, D_CharacterControlPath control, Vector3 targetPos, int maxLoops = 2000, int calcsPerFrame = 10)
    {
        A_Node start = grid.GetNodeByWorldPosition(control.transform.position, true);
        A_Node goal  = grid.GetNodeByWorldPosition(targetPos, true);

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) Debug.Log("Pathfinder: Start(" + start.x + "/" + start.y + ") Goal("+goal.x+"/"+goal.y+")");

        control.StopCoroutine("FollowWayponts"); // <-- this does not happen, but why?

        yield return FindPath(grid, start, goal, control, maxLoops, calcsPerFrame);
        
    }

    // MUSS DAS WIRKLICH EINE COROUTINE SEIN?! Ja .... weil D_Interaction auf den Path warten muss
	public static IEnumerator FindPath(A_Grid grid, A_Node start, A_Node goal, D_CharacterControlPath control, int maxLoops, int calcsPerFrame)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) Debug.Log("Pathfinder: TRYING TO FIND PATH FROM (" + start.x + "/" + start.y + ") TO (" + goal.x + "/" + goal.y + ")");
        List<A_Waypoint> openList = new List<A_Waypoint>();
        List<A_Waypoint> closedList = new List<A_Waypoint>();
        List<A_Node> neighbourNodes;

        A_Waypoint waypointOld;
        A_Waypoint waypointNew;

        A_Waypoint currentWaypoint = new A_Waypoint(start, null, grid.GetDistance(start, goal), false);
        openList.Add(currentWaypoint);

        int loopCount = 0;

        while(openList[0].mNode != goal && openList.Count > 0 && loopCount < maxLoops)
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) Debug.Log("Pathfinder: Looping: " + loopCount + "\n Node: ("+ openList[0].mNode.x + "/"+ openList[0].mNode.y + ")");
            loopCount++;
            if (loopCount % calcsPerFrame == 0)
            {
                Debug.Log("Pathfinder: Waiting for Frame...");
                yield return new WaitForEndOfFrame();
            }
            currentWaypoint = openList[0];

            neighbourNodes = grid.GetNeighboursStraight(currentWaypoint.mNode);
            foreach (A_Node node in neighbourNodes)
            {
                if(GetWaypointWithNode(closedList, node) == null)
                {
                    waypointOld = GetWaypointWithNode(openList, node);
                    if (waypointOld == null)
                    {
                        openList.Add(new A_Waypoint(node, currentWaypoint, grid.GetDistance(node, goal), false));
                    }
                    else
                    {
                        // If we found a path to a node that is essentially faster than the old path, use that new one instead
                        waypointNew = new A_Waypoint(node, currentWaypoint, grid.GetDistance(node, goal), false);
                        if(waypointOld.mCombined < waypointNew.mCombined)
                        {
                            openList.Remove(waypointOld);
                            openList.Add(waypointNew);
                        }
                    }
                }
            }

            neighbourNodes = grid.GetNeighboursDiagnoal(currentWaypoint.mNode);
            foreach (A_Node node in neighbourNodes)
            {
                if (GetWaypointWithNode(closedList, node) == null)
                {
                    waypointOld = GetWaypointWithNode(openList, node);
                    if (waypointOld == null)
                    {
                        openList.Add(new A_Waypoint(node, currentWaypoint, grid.GetDistance(node, goal), true));
                    }
                    else
                    {
                        // If we found a path to a node that is essentially faster than the old path, use that new one instead
                        waypointNew = new A_Waypoint(node, currentWaypoint, grid.GetDistance(node, goal), true);
                        if (waypointOld.mCombined < waypointNew.mCombined)
                        {
                            openList.Remove(waypointOld);
                            openList.Add(waypointNew);
                        }
                    }
                }
            }

            closedList.Add(currentWaypoint);
            openList.Remove(currentWaypoint);

            openList.Sort(SortByScore);

           // Debug.Log(" ============================== ");
           // foreach(A_Waypoint waypoint in openList)
           // {
           //     Debug.Log("(" + waypoint.mNode.x + "/" + waypoint.mNode.y + ") Score: " + waypoint.mCombined);
           // }
        }

        if(openList[0].mNode == goal)
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) Debug.Log("Pathfinder: Success!");

            yield return control.WalkPath(openList[0]);
            yield break;
        }

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Message)) Debug.LogWarning("Pathfinder: Failure!");
        // Finalize when currentWaypoint is goalNode
        // Abort if openList is empty
        yield break;
    }

    static A_Waypoint GetWaypointWithNode(List<A_Waypoint> openList, A_Node node)
    {
        foreach(A_Waypoint waypoint in openList)
        {
            if(waypoint.mNode == node)
            {
                return waypoint;
            }
        }
        return null;
    }

    static int SortByScore(A_Waypoint wp1, A_Waypoint wp2)
    {
        return wp1.mCombined.CompareTo(wp2.mCombined);
    }
}
