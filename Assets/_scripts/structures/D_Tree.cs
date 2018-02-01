using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Tree : D_Structure, D_IInventory, D_IMineable
{
    public float mFruitPosUp = 0.2f;
    public float mFruitPosDown = 0.55f;
    public float mFruitLeft = -0.5f;
    public float mFruitRight = 0.5f;

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
    public List<D_Item> mFruits = new List<D_Item>();
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

    public GameObject    pFruitPrefab;
    public GameObject pResourcePrefab;

    public int mFruitsPerHour = 60;
    public int mMaxFruits = 4;

    public float mFruitSpawnTimer;

    public int mNumberOfSpawnedFruitsTotal = 0;

    public D_Item ProduceResource()
    {
        GameObject itemGO = Instantiate(pResourcePrefab, transform.position + Vector3.up * 0.01f, transform.rotation);
        D_Item item = itemGO.GetComponent<D_Item>();
        item.ClearFlags();
        item.SetFlag(D_StructsAndEnums.EInteractionRestriction.IR_World);

        return itemGO.GetComponent<D_Item>();
    }

    void Update()
    {
        // == Check for Fruit to spawn ==
        if (mFruits.Count < mMaxFruits)
        {
            mFruitSpawnTimer -= Time.deltaTime;

            if (mFruitSpawnTimer < 0f)
            {
                // spawn Fruit
                GameObject fruit = Instantiate(pFruitPrefab, transform);
                fruit.name = fruit.name + " " + mNumberOfSpawnedFruitsTotal;
                mNumberOfSpawnedFruitsTotal++;

                fruit.transform.localEulerAngles = Vector3.zero;
                fruit.transform.localPosition = Vector3.forward * UnityEngine.Random.Range(-0.3f, -0.01f) + Vector3.right * UnityEngine.Random.Range(mFruitLeft, mFruitRight) + Vector3.up * UnityEngine.Random.Range(mFruitPosDown, mFruitPosUp);
                
                fruit.GetComponent<D_Fruit>().AddSelfToInventory(this, false);
                fruit.GetComponent<D_Fruit>().ClearFlags();
                fruit.GetComponent<D_Fruit>().SetFlag(D_StructsAndEnums.EInteractionRestriction.IR_World);

                // reset timer
                mFruitSpawnTimer = 3600f / mFruitsPerHour;
            }


        }

        // == Grow all existing fruit ==
        foreach (D_Fruit fruit in mFruits)
        {
            fruit.Grow();
        }
    }
}
