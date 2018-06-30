using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "A_Pathfinder", menuName = "Pathfinder", order = 66)]
public class A_Pathfinder : ScriptableObject
{
    private static A_Pathfinder PATHFINDER;

    public static A_Pathfinder GetInstance()
    {
        if(PATHFINDER == null)
        {
            A_Pathfinder[] pathfinders = Resources.FindObjectsOfTypeAll<A_Pathfinder>();   // wont work at Runtime: FindObjectOfType<A_Pathfinder>();
            if (pathfinders.Length < 1)
            {
                Debug.LogError("No Pathfinder found!");
                Debug.Break();
            }
            else if (pathfinders.Length > 1)
            {
                Debug.LogError("More than one Pathfinder found!");
                Debug.Break();
            }
            else
            {
                PATHFINDER = pathfinders[0];
            }

        }
        if(PATHFINDER == null)
        {
            PATHFINDER = CreateInstance<A_Pathfinder>();
        }
        return PATHFINDER;
    }

    public void TryPath(A_Grid grid, D_CharacterControlPath control, Vector3 targetPos, int maxLoops = 2000)
    {
        A_Node start = grid.GetNodeByWorldPosition(control.transform.position, true);
        A_Node goal  = grid.GetNodeByWorldPosition(targetPos, true);

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Path_Message)) Debug.Log("Pathfinder: Start(" + start.x + "/" + start.y + ") Goal("+goal.x+"/"+goal.y+")");

        control.StopCoroutine("FollowWayponts"); // <-- this does not happen, but why?

        A_Waypoint path = FindPath(grid, start, goal, control, maxLoops);
        if (path != null)
        {
            control.WalkPath(path);
        }
    }

    // MUSS DAS WIRKLICH EINE COROUTINE SEIN?!
	public A_Waypoint FindPath(A_Grid grid, A_Node start, A_Node goal, D_CharacterControlPath control, int maxLoops)
    {
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Path_Message)) Debug.Log("Pathfinder: TRYING TO FIND PATH FROM (" + start.x + "/" + start.y + ") TO (" + goal.x + "/" + goal.y + ")");
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
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Path_Message)) Debug.Log("Pathfinder: Looping: " + loopCount);
            loopCount++;
           // yield return new WaitForEndOfFrame();
            currentWaypoint = openList[0];
           // currentWaypoint.mNode.mIndicator.GetComponent<Renderer>().material.color = Color.black;
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
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Path_Message)) Debug.Log("Pathfinder: Success!");

            return openList[0];
        }

        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_Path_Message)) Debug.LogWarning("Pathfinder: Failure!");
        // Finalize when currentWaypoint is goalNode
        // Abort if openList is empty
        return null;
    }

    A_Waypoint GetWaypointWithNode(List<A_Waypoint> openList, A_Node node)
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
