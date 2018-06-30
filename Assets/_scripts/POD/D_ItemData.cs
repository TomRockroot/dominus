using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

[CreateAssetMenu(fileName = "D_ItemData", menuName = "Target Data/Item Data", order = 0)]
public class D_ItemData : ScriptableObject
{
    public string mName = "Some Item";      // POD
    public Sprite mInventorySprite;         // POD
    public Material mBillboard;

    public ENodeStatus mOccupyType;         // POD

    public int mParry = 2;                  // POD
    public int mIntegrity = 100;            // POD

    public D_Maslow mOnConsumptionMaslow;   // POD
    public List<D_Interaction> mPossibleInteractions;           // POD

    public float mInteractionRange = 1f;
    public float mInteractionSpeed = 1f;
}
