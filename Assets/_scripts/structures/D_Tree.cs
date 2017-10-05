using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Tree : D_Structure, D_IInventory, D_IMineable
{
    public override void SetIntegrity(int integrity)
    {
        if (integrity <= 0)
        {
            ProduceResource();
        }
        base.SetIntegrity(integrity);
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
     //   Debug.LogError("item: " + item + " item.GetTransform() " + item.GetTransform() + " parent: " + item.GetTransform().parent);
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

    public D_Item ProduceResource()
    {
        GameObject itemGO = Instantiate(pResourcePrefab, transform.position + Vector3.up * 0.01f, transform.rotation);

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
                fruit.transform.position = transform.position + Vector3.up * 0.01f + Vector3.right * 1f * UnityEngine.Random.Range(-1f, 1f) + Vector3.forward * 1f * UnityEngine.Random.Range(-1f, 1f);
                fruit.GetComponent<D_Fruit>().AddToInventory(this);

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

    public override void Interact(D_CharacterControl cntl, D_Interaction interaction)
    {
        // if character has skill
        D_Skill skill = cntl.mCharacter.GetSkill(D_StructsAndEnums.EBonus.B_WoodCutting);
        if (skill != null)
        {
            skill.ExecuteSkill(this);
        }
    }

    void OnDestroy()
    {
        DropInventory();
    }
}
