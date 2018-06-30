using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "D_StructureData", menuName = "Target Data/Structure Data", order = 1)]
public class D_StructureData : ScriptableObject
{
    public string mName = "Some Structure"; // POD
    public Sprite mCraftSprite;             // POD
    public Material mBillboard;

    public ENodeStatus mOccupyType;         // POD

    public int mParry = 2;                  // POD
    public int mIntegrity = 100;            // POD

    public D_Maslow mOnUseMaslow;           // POD
    public List<D_Interaction> mPossibleInteractions;

    public float mInteractionRange = 1f;
    public float mInteractionSpeed = 1f;
}
