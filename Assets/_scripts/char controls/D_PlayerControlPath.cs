using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerControlPath : D_PlayerControl
{
    public void FindPathTo(A_Grid grid, Vector3 worldPos)
    {
        StopAllCoroutines();
        StartCoroutine( A_Pathfinder.TryPath(grid, this, worldPos) );
    }

    public IEnumerator FindPathToCoroutine(A_Grid grid, Vector3 worldPos)
    {
        yield return StartCoroutine(A_Pathfinder.TryPath(grid, this, worldPos));
    }
}
