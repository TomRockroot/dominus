using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Grid
{
    public float mNodeDistance = 1f;
    public Terrain mTerrain;

    public A_Node[,] mNodes; 

    private int mNumHorizontal = -1;
    private int mNumVertical = -1;

    public A_Grid(float nodeDistance, Terrain terrain)
    {
        mNodeDistance = nodeDistance;
        mTerrain = terrain;

        CreateGrid();
    }

    public float GetDistance(A_Node from, A_Node to)
    {
          Vector2 vector = new Vector2((to.x - from.x) * mNodeDistance, (to.y - from.y) * mNodeDistance);
      //  Vector2 vector = new Vector2((to.x - from.x) * mNodeDistance * 2f, (to.y - from.y) * mNodeDistance * 2f); // WEIRD SHIT
        return vector.magnitude;
    }

    public List<A_Node> GetNeighboursStraight(A_Node original)
    {
        List<A_Node> neighbours = new List<A_Node>();

        // Left
        if (original.x > 0)
        {
            neighbours.Add( mNodes[original.x - 1, original.y] );
        }

        // Right
        if (original.x < mNumHorizontal - 1)
        {
            neighbours.Add(mNodes[original.x + 1, original.y]);
        }

        // Bottom
        if(original.y > 0)
        {
            neighbours.Add(mNodes[original.x, original.y - 1]);
        }

        // Top
        if (original.y < mNumVertical - 1)
        {
            neighbours.Add(mNodes[original.x, original.y + 1]);
        }
        


        return neighbours;
    }

    public List<A_Node> GetNeighboursDiagnoal(A_Node original)
    {
        List<A_Node> neighbours = new List<A_Node>();

        // Bottom Left
        if(original.x > 0 && original.y > 0)
        {
            neighbours.Add(mNodes[original.x - 1, original.y - 1]);
        }

        // Bottom Right
        if(original.x > 0 && original.y < mNumVertical - 1)
        {
            neighbours.Add(mNodes[original.x - 1, original.y + 1]);
        }

        // Top Left
        if(original.x < mNumHorizontal - 1 && original.y > 0)
        {
            neighbours.Add(mNodes[original.x + 1, original.y - 1]);
        }

        // Top Right
        if(original.x < mNumHorizontal - 1 && original.y < mNumVertical - 1)
        {
            neighbours.Add(mNodes[original.x + 1, original.y + 1]);
        }

        return neighbours;
    }

    public void CreateGrid()
    {
        float width = mTerrain.terrainData.size.x;
        float height = mTerrain.terrainData.size.z;

        int numHorizontal = Mathf.FloorToInt(  width / mNodeDistance );
        int numVertical   = Mathf.FloorToInt( height / mNodeDistance );

        mNumHorizontal = numHorizontal;
        mNumVertical = numVertical;

        mNodes = new A_Node[numHorizontal,numVertical];

        for(int i = 0; i < numHorizontal; i++)
        {
            for(int k = 0; k < numVertical; k++)
            {
                mNodes[i, k] = new A_Node(i, k, this);
            }
        }

        Debug.Log("Grid created! H: (" + numHorizontal + ") V: (" + numVertical + ")");
    }

    public A_Grid GetSubgrid(A_Node bottomLeft, A_Node topRight)
    {
        A_Grid subgrid = new A_Grid(mNodeDistance, mTerrain);
        subgrid.mNodes = new A_Node[topRight.x - bottomLeft.x, topRight.y - bottomLeft.y];

        for (int i = bottomLeft.x; i < topRight.x; i++ )
        {
            for (int k = bottomLeft.y; k < topRight.y; k++ )
            {
                subgrid.mNodes[i - bottomLeft.x, k - bottomLeft.y] = mNodes[i, k];
            }
        }


        return subgrid;
    }
}
