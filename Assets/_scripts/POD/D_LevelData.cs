using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;
using System;

public class D_LevelData : MonoBehaviour
{
    public string mID;

    public Terrain mTerrain;
    public A_Grid  mGrid;
    public List<D_Item>        mItems;
    public List<D_Structure>   mStructures;
    public List<D_Character>   mCharacters;

    void Awake()
    {
        D_GameMaster.GetInstance().mAllLevels.Add(this);
    }

    void OnDestroy()
    {
        D_GameMaster.GetInstance().mAllLevels.Remove(this);
    }
}
