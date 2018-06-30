using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class A_GridDrawer : MonoBehaviour {

    public A_Grid mGrid;
    public bool bShowGrid;

    public void DrawGrid(A_Grid grid)
    {
        Debug.Log("DrawGrid");
        mGrid = grid;
    }
}
