using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(A_GridDrawer))]
public class A_GridDrawerInspector : Editor
{
    public Camera mSceneCamera;

    void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (((A_GridDrawer)target).bShowGrid )
            {
                if (((A_GridDrawer)target).mGrid != null)
                {
                    A_Grid grid = ((A_GridDrawer)target).mGrid;
                    // Debug.LogWarning("~~~" + grid);
                    // Debug.LogWarning("~~~" + grid.mNodes);
                    // Debug.LogWarning("~~~" + grid.mNodes.Length); 
                    /* foreach(A_Node node in grid.mNodes)
                     {
                         Debug.LogWarning("xxx " + node);
                         Debug.LogWarning("xxx (" + node.x + "/" + node.y + ")");
                         Debug.LogWarning("xxx " + node.mStatus);
                         break;
                     }
                     */
                    Vector3 worldPosNode;
                    Vector3 worldPosTop;
                    Vector3 worldPosRight;

                    foreach (A_Node node in grid.mNodes) 
                    {
                        A_Node top = grid.GetNeighbourTop(node);
                        A_Node right = grid.GetNeighbourRight(node);

                        worldPosNode = grid.GetWorldPositionByNode(node);

                        if(mSceneCamera == null)
                        {
                            mSceneCamera = SceneView.currentDrawingSceneView.camera;
                        }

                        if (mSceneCamera != null)
                        {
                            
                            if (mSceneCamera.WorldToViewportPoint(worldPosNode).x < 0 ||
                                mSceneCamera.WorldToViewportPoint(worldPosNode).y < 0 ||
                                mSceneCamera.WorldToViewportPoint(worldPosNode).x > 1 ||
                                mSceneCamera.WorldToViewportPoint(worldPosNode).y > 1)
                            {
                                continue;
                            }
                        }

                        switch (node.mStatus)
                        {
                            case D_StructsAndEnums.ENodeStatus.NS_Normal:
                                Handles.color = Color.white;
                                break;

                            case D_StructsAndEnums.ENodeStatus.NS_Fast:
                                Handles.color = Color.green;
                                break;

                            case D_StructsAndEnums.ENodeStatus.NS_Slow:
                                Handles.color = Color.yellow;
                                break;

                            case D_StructsAndEnums.ENodeStatus.NS_Blocked:
                                Handles.color = Color.red;
                                break;

                            case D_StructsAndEnums.ENodeStatus.NS_Occupied:
                                Handles.color = Color.blue;
                                break;
                        }

                        if (top != null)
                        {
                            worldPosTop = grid.GetWorldPositionByNode(top);
                            Handles.DrawLine(worldPosNode, worldPosTop);
                        }
                        if (right != null)
                        {
                            worldPosRight = grid.GetWorldPositionByNode(right);
                            Handles.DrawLine(worldPosNode, worldPosRight);
                        }
                    }
                }
            }
        }
    }

}
