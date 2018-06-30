using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Tree : D_Structure, D_IInventory, D_IMineable
{
    [SerializeField]
    protected D_GrowthData mGrowthData;
    
    public D_GrowthData GetGrowthData()
    {
        return mGrowthData;
    }

    private float mFruitSpawnTimer;
    private int mNumberOfSpawnedFruitsTotal = 0;

    public override int SetIntegrity(int integrity)
    {
        if (integrity <= 0)
        {
            ProduceResource();
            DropInventory();
        }
        return base.SetIntegrity(integrity);
    }

    // == Inventory ==
    private List<D_Item> mFruits = new List<D_Item>();
    public List<D_Item> GetInventory() { return mFruits; }
    public void AddToInventory(D_Item item)
    {
        mFruits.Add(item);
    }
    public void RemoveFromInventory(D_Item item)
    {
        mFruits.Remove(item);
        item.GetTransform().parent = transform.parent;
    }

    public void DropInventory()
    {
        foreach (D_Item item in mFruits)
        {
            item.GetTransform().parent = transform.parent;
        }
    }

    public D_Item ProduceResource()
    {
        GameObject itemGO = Instantiate(D_GameMaster.GetInstance().pItem.gameObject, transform.position + Vector3.up * 0.01f, transform.rotation);
     // 
        D_Item item = itemGO.GetComponent<D_Item>();
        item.SetData(GetGrowthData().pResourceData);
        item.ClearFlags();
        item.SetFlag(D_StructsAndEnums.EInteractionRestriction.IR_World);

        return itemGO.GetComponent<D_Item>();
    }

    void Update()
    {
        // == Check for Fruit to spawn ==
        if (mFruits.Count < GetGrowthData().mMaxFruits)
        {
           mFruitSpawnTimer -= Time.deltaTime;

            if (mFruitSpawnTimer < 0f)
            {
                // spawn Fruit
                GameObject fruitGO = Instantiate(D_GameMaster.GetInstance().pFruit.gameObject, transform);
                D_Fruit fruit = fruitGO.GetComponent<D_Fruit>();
                fruit.SetData(GetGrowthData().pFruitData);
                // !!!!!!!!!!!!!!!!


                fruit.name = fruit.GetData().mName + " " + mNumberOfSpawnedFruitsTotal;
                mNumberOfSpawnedFruitsTotal++;

                fruit.transform.localEulerAngles = Vector3.zero;
                fruit.transform.localPosition = Vector3.forward * UnityEngine.Random.Range(-0.3f, -0.01f) + Vector3.right * UnityEngine.Random.Range(GetGrowthData().mFruitLeft, GetGrowthData().mFruitRight) + Vector3.up * UnityEngine.Random.Range(GetGrowthData().mFruitPosDown, GetGrowthData().mFruitPosUp);
                
                fruit.AddSelfToInventory(this, false);
                fruit.ClearFlags();
                fruit.SetFlag(D_StructsAndEnums.EInteractionRestriction.IR_World);

                // reset timer
                mFruitSpawnTimer = 3600f / GetGrowthData().mFruitsPerHour;
            }


        }

        // == Grow all existing fruit ==
        foreach (D_Fruit fruit in mFruits)
        {
            fruit.Grow(GetGrowthData());
        }
    }
}
