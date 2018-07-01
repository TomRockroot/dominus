using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;
using System;

public class D_GameMaster : MonoBehaviour, D_IInventory
{
    public D_Character pCharacter;
    public D_Structure pStructure;
    public D_Item pItem;

    public D_Tree pTree;
    public D_Fruit pFruit;

    public List<D_LevelData> mAllLevels = new List<D_LevelData>();
    private D_LevelData mCurrentLevel;

    public D_LevelData GetCurrentLevel()
    {
        if(mCurrentLevel == null)
        {
            foreach(D_LevelData level in mAllLevels)
            {
                mCurrentLevel = level;
                break;
            }
        }
        return mCurrentLevel;
    }

    [HideInInspector]
    public EDebugLevel mDebugFlags = 0;

    private List<D_ITargetable> mAllTargetables = new List<D_ITargetable>();
    private D_PlayerControl mCurrentController;

    private List<D_Item> mWorldInventory = new List<D_Item>();

    public float mGameMoveSpeed = 1.0f;

    public bool IsFlagged(EDebugLevel flag)
    {
        return (mDebugFlags & flag) == flag;
    }

    public float GetGameMoveSpeed()
    {
        return mGameMoveSpeed;
    }

    public void SetCurrentController(D_PlayerControl controller)
    {
        mCurrentController = controller;
    }

    public D_PlayerControl GetCurrentController()
    {
        return mCurrentController;
    }


    public void RegisterTargetable(D_ITargetable targetable)
    {
        if(mAllTargetables.Contains(targetable))
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Error)) Debug.LogError("General: Tried to add ITargetable " + targetable.GetTransform().name + " twice!\nDENIED!");
            return;
        }
        mAllTargetables.Add(targetable);
        if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Warning)) Debug.LogWarning("General: AVAILABLE TARGETS: " + mAllTargetables.Count);
    }

    public void UnregisterTargetable(D_ITargetable targetable)
    {
        if (!mAllTargetables.Contains(targetable))
        {
            if (D_GameMaster.GetInstance().IsFlagged(EDebugLevel.DL_General_Error)) Debug.LogError("General: Tried to remove ITargetable " + targetable.GetTransform().name + " when it's not there anymore!\nDENIED!");
            return;
        }
        mAllTargetables.Remove(targetable);
      //  Debug.LogWarning("AVAILABLE TARGETS: " + mAllTargetables.Count);
    }

    public List<D_ITargetable> GetAllTargetables()
    {
        mAllTargetables.RemoveAll(D_ITargetable => D_ITargetable == null);
        return mAllTargetables;
    }

    // ==== SINGLETON SHIT ====     https://youtu.be/6vmRwLYWNRo?t=30m22s 
    private static D_GameMaster GAME_MASTER;

    public static D_GameMaster GetInstance() 
    {
        if(GAME_MASTER == null)
        {
            Debug.LogWarning("GameMaster: was null!");
            GAME_MASTER = GameObject.FindObjectOfType<D_GameMaster>();

            if(GAME_MASTER == null)
            {
                Debug.LogWarning("GameMaster: Not found!");
                Debug.Break();
            }
        }
        return GAME_MASTER; 
    }

    public D_Structure CreateStructure(D_StructureData data)
    {
        D_Structure structure = Instantiate(pStructure);
        structure.SetData(data);
        return structure;
    }

    public D_LevelData CreateLevel(string id)
    {
        Debug.Log("GameMaster: Creating a Level_" + id + " !");

        GameObject go = new GameObject();
        D_LevelData levelData = go.AddComponent<D_LevelData>();
        levelData.mID = id;
        go.name = "Level_" + id;

        mAllLevels.Add(levelData);

        return levelData;
    }

    public void SaveLevel(D_LevelData levelData)
    {
        SOS_Level.Save(levelData);
    }

    public D_LevelData LoadLevel(string id, D_LevelData levelData = null)
    {
        if(levelData == null)
        {
            levelData = CreateLevel(id);
        }
        SOS_Level.Load(id, levelData);

        return levelData;
    }

    public void RemoveFromInventory(D_Item item)
    {
        mWorldInventory.Remove(item);
    }

    public void AddToInventory(D_Item item)
    {
        mWorldInventory.Add(item);
    }

    public void DropInventory()
    {
        throw new NotImplementedException();
    }

    public List<D_Item> GetInventory()
    {
        return mWorldInventory;
    }

    public Transform GetTransform()
    {
        Debug.Break();
        return null; // DONT TOUCH MA SPAGETT
    }
}
