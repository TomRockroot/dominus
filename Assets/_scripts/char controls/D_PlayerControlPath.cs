using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerControlPath : D_PlayerControl
{
    public void FindPathTo(A_Grid grid, Vector3 worldPos)
    {
        A_Pathfinder.GetInstance().TryPath(grid, this, worldPos);
    }

}
