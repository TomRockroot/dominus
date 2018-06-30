using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class A_Grid : MonoBehaviour
{
    public int mID;
    public Terrain mTerrain;

    public float mNodeDistance = 1f;
    public int mNumHorizontal = -1;
    public int mNumVertical = -1;

    public A_Node[] mNodes;  

    // Constructor
    public bool Inititalize(float nodeDistance, Terrain terrain)
    {
        if(nodeDistance > 0)
        {
            mNodeDistance = nodeDistance;
            mTerrain = terrain;

            mTerrain.GetComponent<D_TerrainClick>().SetGrid(this); // <-- Problem: Multiple Terrains
            CreateGrid();

            return true;
        }

        return false;
    }

    // Destructor
    private void OnDestroy()
    {
        Debug.LogWarning("Grid " + mID + " is being destroyed! <(o.o<)");
        int i = 0;
        foreach (A_Node node in mNodes)
        {
            i++;
            DestroyImmediate(node);
        }

        Debug.LogWarning("Grid " + mID + " and " + i +" nodes were destroyed! (>o.o)>");
    }

    void OnEnable()
    {
        Debug.Log(name + " was enabled! <(o.o<)");
    }

    public float GetDistance(A_Node from, A_Node to)
    {
          Vector2 vector = new Vector2((to.x - from.x) * mNodeDistance, (to.y - from.y) * mNodeDistance);
      //  Vector2 vector = new Vector2((to.x - from.x) * mNodeDistance * 2f, (to.y - from.y) * mNodeDistance * 2f); // WEIRD SHIT   <-- wtf is this comment?
        return vector.magnitude;
    }

    public Vector3 GetWorldPositionByNode(A_Node node)
    {
        return GetWorldPositionByNode(node.x, node.y);
    }

    public Vector3 GetWorldPositionByNode(int i, int k)
    {
        return mTerrain.GetPosition() + new Vector3(mNodeDistance * 0.5f + i * mNodeDistance,
                                                                        mTerrain.SampleHeight(mTerrain.transform.position + new Vector3(mNodeDistance * 0.5f + i * mNodeDistance, 0, mNodeDistance * 0.5f + k * mNodeDistance)),
                                                                        mNodeDistance * 0.5f + k * mNodeDistance);
    }

    public A_Node GetNodeByWorldPosition(Vector3 pos, bool forceNode = false)
    {
        Vector3 local = pos - mTerrain.GetPosition();
        Vector3 normed = local * 1 / mNodeDistance;
        
        int i = Mathf.RoundToInt(normed.x);
        int k = Mathf.RoundToInt(normed.z);

        Debug.Log("Normed: " + normed + "\nInt: (" + i +"/"+ k +")"  );
        Debug.Log("mNumHorizontal: " + mNumHorizontal + "\n mNumVertical: " + mNumVertical);

        if (i >= 0 && i < mNumHorizontal && 
            k >= 0 && k < mNumVertical)
        {
            return GetNodeAt(i, k);
        }
        else
        {
            if (forceNode)
            {
                if (i < 0) i = 0;
                if (k < 0) k = 0;
                if (i >= mNumHorizontal) i = mNumHorizontal - 1;
                if (k >= mNumVertical) k = mNumVertical - 1;

                return GetNodeAt(i, k);
            }
            else
            {
                Debug.LogWarning("^(o.o)^");
                return null;
            }
        }
    }

    public A_Node GetNeighbourTop(A_Node original)
    {
        // Top
        if (original.y < mNumVertical - 1)
        {
            return GetNodeAt(original.x, original.y + 1);
        }
         
        return null;
    }

    public A_Node GetNeighbourRight(A_Node original)
    {
        // Right
        if (original.x < mNumHorizontal - 1)
        {
            return GetNodeAt(original.x + 1, original.y);
        }

        return null;
    }

    public List<A_Node> GetNeighboursStraight(A_Node original)
    {
        List<A_Node> neighbours = new List<A_Node>();

        // Left
        if (original.x > 0)
        {
            neighbours.Add(GetNodeAt(original.x - 1, original.y) );
        }

        // Right
        if (original.x < mNumHorizontal - 1)
        {
            neighbours.Add(GetNodeAt(original.x + 1, original.y) );
        }

        // Bottom
        if(original.y > 0)
        {
            neighbours.Add(GetNodeAt(original.x, original.y - 1) );
        }

        // Top
        if (original.y < mNumVertical - 1)
        {
            neighbours.Add(GetNodeAt(original.x, original.y + 1) );
        }
        


        return neighbours;
    }

    public List<A_Node> GetNeighboursDiagnoal(A_Node original)
    {
        List<A_Node> neighbours = new List<A_Node>();

        // Bottom Left
        if(original.x > 0 && original.y > 0)
        {
            neighbours.Add(GetNodeAt(original.x - 1, original.y - 1) );
        }

        // Bottom Right
        if(original.x > 0 && original.y < mNumVertical - 1)
        {
            neighbours.Add(GetNodeAt(original.x - 1, original.y + 1) );
        }

        // Top Left
        if(original.x < mNumHorizontal - 1 && original.y > 0)
        {
            neighbours.Add(GetNodeAt(original.x + 1, original.y - 1) );
        }

        // Top Right
        if(original.x < mNumHorizontal - 1 && original.y < mNumVertical - 1)
        {
            neighbours.Add(GetNodeAt(original.x + 1, original.y + 1));
        }

        return neighbours;
    }

    public A_Node GetNodeAt(int x, int y)
    {
        return mNodes[mNumHorizontal * y + x];
    }

    public static void SnapToGrid(D_ITargetable target, A_Grid grid)
    {
        A_Node node = grid.GetNodeByWorldPosition(target.GetTransform().position);

        if (node != null)
        {
            target.GetTransform().position = grid.GetWorldPositionByNode(node);

            target.SetNode(node);
        }
    }

    public void CreateGrid()
    {
        float width = mTerrain.terrainData.size.x;
        float height = mTerrain.terrainData.size.z;

        int numHorizontal = Mathf.FloorToInt(  width / mNodeDistance );
        int numVertical   = Mathf.FloorToInt( height / mNodeDistance );

        mNumHorizontal = numHorizontal;
        mNumVertical = numVertical;

        if(mNodes != null)
        {
            foreach(A_Node spareNode in mNodes)
            {
                DestroyImmediate(spareNode);
            }
        }

        mNodes = new A_Node[numHorizontal * numVertical];

        A_Node node;
        
        for(int k = 0; k < numVertical ; k++)
        {
            for (int i = 0; i < numHorizontal; i++)
            {
                node = ScriptableObject.CreateInstance<A_Node>();
                node.Initialize(i, k, this); 
                   // new A_Node(i, k, this);

                mNodes[numHorizontal * k + i] = node;
            }
        }

        Debug.Log("Grid created! H: (" + numHorizontal + ") V: (" + numVertical + ")\nArrayLength: " + mNodes.Length);
    }

    public void SaveGrid(string path)
    {
        // JsonUtility.ToJson(this);
#if UNITY_EDITOR
        // if asset already exists
       /* if(UnityEditor.AssetDatabase.)
        {
            delete it
            UnityEditor.AssetDatabase.DeleteAsset(path);
        }
        */
        // and write it anew
        UnityEditor.AssetDatabase.CreateAsset(this, path + ".grid");
#endif
        Debug.LogWarning("Grid saved!");
    }

    public A_Grid LoadGrid(string path)
    {
        // var json = File.ReadAllText(path);
        // JsonUtility.FromJsonOverwrite(json, this);
        AssetBundle.LoadFromFile(path + ".grid");
        return this;
    }
}
