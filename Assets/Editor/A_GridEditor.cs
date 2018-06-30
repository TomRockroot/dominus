using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class A_GridEditor : EditorWindow
{
    enum ESelectionType
    {
        ST_None,
        ST_Terrain,
        ST_Targetable,
        ST_TargetMulti
    }
    private ESelectionType mSelection = ESelectionType.ST_None;


    bool bOverrideGrid = false;

    private Terrain mTerrainSelected;
    private float mNodeDistance = 1.0f;
    [SerializeField]
    private A_Grid mGrid;
    private string mGridName = "someGrid";
    
    // Control Panel Entry Data
    private int mEntryOffset =  3;
    private int mEntryHeight = 20;
    private int mEntryPadding=  3;


    [MenuItem ("Window/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(A_GridEditor));
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.GetComponent<Terrain>() != null)
            {
                mSelection = ESelectionType.ST_Terrain;
            }
            else if (Selection.gameObjects.Length > 1)
            {
                bool clean = true;

                foreach(GameObject go in Selection.gameObjects)
                {
                    clean = clean && (go.GetComponent<D_ITargetable>() != null);
                    if(!clean)
                    {
                        break;
                    }
                }

                if(clean)
                {
                    mSelection = ESelectionType.ST_TargetMulti;
                }
                else
                {
                    mSelection = ESelectionType.ST_None;
                }
            }
            else
            {
                if (Selection.activeGameObject.GetComponent<D_ITargetable>() != null )
                {
                    mSelection = ESelectionType.ST_Targetable;
                }
                else
                {
                    mSelection = ESelectionType.ST_None;
                }
            }

            int entryCount = 0;
            EditorGUI.LabelField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, 20), "Selection " + mSelection);
            entryCount++;


            switch (mSelection)
            {
                case ESelectionType.ST_None:
                    EditorGUI.LabelField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, 20), "Select Terrain/Target to Start!");
                    entryCount++;
                    break;

                case ESelectionType.ST_Terrain:

                    //if the selected object is a Terrain
                    if (Selection.activeGameObject.GetComponent<Terrain>() != null)
                    {
                        if (mTerrainSelected != Selection.activeGameObject.GetComponent<Terrain>())
                        {
                            mTerrainSelected = Selection.activeGameObject.GetComponent<Terrain>();
                        }


                        // Buttons etc.
                        

                        EditorGUI.LabelField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Grid");
                        entryCount++;

                        mGridName = EditorGUI.TextField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Name", mGridName);
                        entryCount++;

                        mTerrainSelected = (Terrain)EditorGUI.ObjectField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Terrain", mTerrainSelected, typeof(Terrain), true);
                        entryCount++;

                        mNodeDistance = EditorGUI.FloatField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Node Distance", mNodeDistance);
                        entryCount++;

                        // Create Grid
                        if (bOverrideGrid)
                        {
                            if (mNodeDistance > 0)
                            {
                                if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6) * 0.5f, mEntryHeight), "CREATE"))
                                {
                                    CreateGrid();
                                }
                            } 
                            else
                            {
                                EditorGUI.LabelField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, 20), "Distance > 1 !!");
                            }
                            if (GUI.Button(new Rect(3 + (position.width - 6) * 0.5f, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6) * 0.5f, mEntryHeight), "CANCEL"))
                            {
                                bOverrideGrid = false;
                            }
                        }
                        else
                        {
                            if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Create"))
                            {
                                CreateGrid();
                            }
                        }

                        entryCount++;

                        // Save Grid
                        if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Save"))
                        {
                            SaveGrid();
                        }
                        entryCount++;

                        // Load Grid
                        if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Load"))
                        {
                            LoadGrid();
                        }
                        entryCount++;

                        // Check Grid
                        if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6) * 0.5f, mEntryHeight), "Check Grid"))
                        {
                            CheckGrid();
                        }
                        // entryCount++; CheckGrid and CheckNode on the same height

                        // Check Nodes
                        if (GUI.Button(new Rect(3 + (position.width - 6) * 0.5f, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6) * 0.5f, mEntryHeight), "Check Nodes"))
                        {
                            CheckNodes();
                        }
                        entryCount++;


                    }
                    break; 

                case ESelectionType.ST_Targetable:
                    if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Snap To Grid"))
                    {
                        //Grid Snapping
                        SnapToGrid(Selection.activeGameObject.GetComponent<D_ITargetable>());
                    }
                    entryCount++; 
                    break;

                case ESelectionType.ST_TargetMulti:
                    if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Snap ALL ("+ Selection.gameObjects.Length +") To Grid"))
                    {
                        //Grid Snapping (Multi)
                        foreach(GameObject go in Selection.gameObjects)
                        {
                            SnapToGrid( go.GetComponent<D_ITargetable>() );
                        }
                    }
                    entryCount++;
                    break;
            }
        }
        
        else
        {
            EditorGUI.LabelField(new Rect(3, 3, position.width - 6, 20), "Select Terrain/Target to Start!");
        }
    
    }

    private void SnapToGrid(D_ITargetable target)
    {
        A_Node node = mGrid.GetNodeByWorldPosition(target.GetTransform().position);

        if (node != null)
        {
            target.GetTransform().position = mGrid.GetWorldPositionByNode(node);

            target.SetNode(node);
        }
    }

    private void CreateGrid()
    {
        Debug.Log("Creating Grid!");

        if (mGrid != null && !bOverrideGrid)
        {
            Debug.LogWarning("There was another Grid! O.o");
            bOverrideGrid = true;
            return;
        }

        if (mTerrainSelected == null)
        {
            Debug.LogError("No Terrain selected to create a A_Grid for! \nHow did you do that?!");
            return;
        }

        if (mTerrainSelected.GetComponent<A_Grid>() == null)
        { 
            mGrid = mTerrainSelected.gameObject.AddComponent<A_Grid>();  // CreateInstance<A_Grid>();
        }
        else
        {
            mGrid = mTerrainSelected.GetComponent<A_Grid>();
        }

        if (mGrid.Inititalize(mNodeDistance, mTerrainSelected))
        {
            mTerrainSelected.GetComponent<A_GridDrawer>().DrawGrid(mGrid);
        }
        bOverrideGrid = false;

       // AssetDatabase.CreateAsset(mGrid, "Assets/_grids/"+ mGridName + ".grid");
    }

    private void SaveGrid()
    {
        Debug.Log("Saving Grid!");
        mGrid.SaveGrid("Assets/_grids/" + mGridName);
    }

    private void LoadGrid()
    {
        Debug.Log("Loading Grid!");
        mGrid = mGrid.LoadGrid("Assets/_grids/" + mGridName);
    }

    private void CheckGrid()
    {
        Debug.LogWarning("--- " + mGrid);
        if(mGrid.mNodes == null)
        {
            Debug.LogWarning("--- == null");
        }
        if(mGrid.mNodes.Equals(null))
        {
            Debug.LogWarning("--- .Equals(null)");
        }
        Debug.LogWarning("--- " + mGrid.mNodes);
        Debug.LogWarning("--- " + mGrid.mNodes.Length);
    }

    private void CheckNodes()
    {
        int i = 0;
        foreach(A_Node node in mGrid.mNodes)
        {
            Debug.LogWarning("^"+i+"^ " + node +" (" + node.x + "/" + node.y + ")\nStatus: " + node.mStatus + " Occupants: " + node.GetOccupants().Count);
            i++;
        }
    }
}