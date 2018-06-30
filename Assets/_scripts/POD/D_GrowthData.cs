using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "D_GrowthData", menuName = "Target Data/Growth Data", order = 4)]
public class D_GrowthData : ScriptableObject
{
    public D_ItemData pFruitData;
    public D_ItemData pResourceData;

    public float mFruitPosUp = 0.2f;
    public float mFruitPosDown = 0.55f;
    public float mFruitLeft = -0.5f;
    public float mFruitRight = 0.5f;

    public Color mRipeColor = Color.green;
    [Range(0f, 1f)]
    public float mGrowthRate = 0.1f;

    public int mFruitsPerHour = 60;
    public int mMaxFruits = 4;

}
