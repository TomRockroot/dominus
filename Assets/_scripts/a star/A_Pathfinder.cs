using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class A_Pathfinder : MonoBehaviour
{
    public Terrain mTerrain;
    public float mNodeDistance;

    public A_Indicator mNodeIndicator;
    List<A_Indicator> mNodeIndicators = new List<A_Indicator>();

    public A_Node mStart;
    public A_Node mGoal;

    public A_Walker mWalker;

    public int mMaxLoops = 2000;

    A_Grid mGrid;

    public void SetStartNode(A_Node node)
    {
        if (mStart != null || mStart == node)
        {
            mStart.mIndicator.SetMode(ENodeManipulation.NM_Normal); // <<--- THIS COULD LEAD TO ERRORS ALONG THE LINE ( need to recreate old status ) 
        }

        mStart = node;
    }

    public void SetGoalNode(A_Node node)
    {
        if (mGoal != null || mGoal == node)
        {
            mGoal.mIndicator.SetMode(ENodeManipulation.NM_Normal);
        }

        mGoal = node;
    }

    public void FindPathButton()
    {
        if(mStart != null && mGoal != null)
        {
            StartCoroutine(FindPath(mGrid, mStart, mGoal));
        }
    }

	public IEnumerator FindPath(A_Grid grid, A_Node start, A_Node goal)
    {
        Debug.Log("TRYING TO FIND PATH FROM (" + start.x + "/" + start.y + ") TO (" + goal.x + "/" + goal.y + ")");
        List<A_Waypoint> openList = new List<A_Waypoint>();
        List<A_Waypoint> closedList = new List<A_Waypoint>();
        List<A_Node> neighbourNodes;

        A_Waypoint currentWaypoint = new A_Waypoint(start, null, grid.GetDistance(start, goal), false);
        openList.Add(currentWaypoint);

        int loopCount = 0;

        while(openList[0].mNode != goal && openList.Count > 0 && loopCount < mMaxLoops)
        {
            Debug.Log("Looping: " + loopCount);
            loopCount++;
            yield return new WaitForEndOfFrame();
            currentWaypoint = openList[0];
            currentWaypoint.mNode.mIndicator.GetComponent<Renderer>().material.color = Color.black;
            neighbourNodes = mGrid.GetNeighboursStraight(currentWaypoint.mNode);
            foreach (A_Node node in neighbourNodes)
            {
                if (!ContainsWaypointWithNode(openList, node) && !ContainsWaypointWithNode(closedList, node))
                {
                    openList.Add(new A_Waypoint(node, currentWaypoint, grid.GetDistance(node, goal), false));
                }
            }

            neighbourNodes = mGrid.GetNeighboursDiagnoal(currentWaypoint.mNode);
            foreach (A_Node node in neighbourNodes)
            {
                if (!ContainsWaypointWithNode(openList, node) && !ContainsWaypointWithNode(closedList, node))
                {
                    openList.Add(new A_Waypoint(node, currentWaypoint, grid.GetDistance(node, goal), true));
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
            Debug.Log("Success!");

            if(mWalker != null)
            {
                mWalker.WalkPath(openList[0]);
            }
        }

        Debug.Log("Done!");
        // Finalize when currentWaypoint is goalNode
        // Abort if openList is empty
    }

    bool ContainsWaypointWithNode(List<A_Waypoint> openList, A_Node node)
    {
        foreach(A_Waypoint waypoint in openList)
        {
            if(waypoint.mNode == node)
            {
                return true;
            }
        }
        return false;
    }

    static int SortByScore(A_Waypoint wp1, A_Waypoint wp2)
    {
        return wp1.mCombined.CompareTo(wp2.mCombined);
    }

    public void CreateGrid()
    { 
        if(mNodeDistance <= 0f || mTerrain == null)
        {
            Debug.LogError("A* Pathfinder - Members on " + name + " not set up properly!");
            return;
        }

        mGrid = new A_Grid(mNodeDistance, mTerrain);
        StartCoroutine(ShowLastDeltaTime());
        Debug.Log("So lange hats gedauert: " + Time.deltaTime);
    }

    IEnumerator ShowLastDeltaTime()
    {
        yield return new WaitForEndOfFrame();
        
    }

    public void ShowGrid()
    {
        HideGrid();

        A_Indicator indicator;

        foreach ( A_Node node in mGrid.mNodes )
        {
            indicator = Instantiate(mNodeIndicator, mTerrain.transform);
            indicator.transform.localPosition = new Vector3(node.x * mNodeDistance, mTerrain.SampleHeight(mTerrain.transform.position + new Vector3(node.x * mNodeDistance, 0, node.y * mNodeDistance)), node.y * mNodeDistance);

            // ===== IS THIS GOOD ? =====
            indicator.mNode = node;
            node.mIndicator = indicator;
            // ======  OR BAD ???  ======

            mNodeIndicators.Add(indicator);
        }
    }

    public void HideGrid()
    {
        foreach(A_Indicator indicator in mNodeIndicators)
        {
            Destroy(indicator.gameObject);
        }
    }
}
