using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class A_LevelEditor : EditorWindow
{

    private D_LevelData mCurrentLevel;
    private string mLevelID = "Zero";

    // Control Panel Entry Data
    private int mEntryOffset = 3;
    private int mEntryHeight = 20;
    private int mEntryPadding = 3;

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(A_LevelEditor));
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        int entryCount = 0;
        EditorGUI.LabelField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, 20), "Level_" + mLevelID);
        entryCount++;

        mLevelID = EditorGUI.TextField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "ID: ", mLevelID);
        entryCount++;

        if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6), mEntryHeight), "Create Empty Level"))
        {
            mCurrentLevel = D_GameMaster.GetInstance().CreateLevel(mLevelID);
        }
        entryCount++;

        mCurrentLevel = (D_LevelData)EditorGUI.ObjectField(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), position.width - 6, mEntryHeight), "Current", mCurrentLevel, typeof(D_LevelData), true);
        entryCount++;

        if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6), mEntryHeight), "Save Level"))
        {
            D_GameMaster.GetInstance().SaveLevel(mCurrentLevel);
        }
        entryCount++;

        if (GUI.Button(new Rect(3, mEntryOffset + entryCount * (mEntryHeight + mEntryPadding), (position.width - 6), mEntryHeight), "Load Level"))
        {
            D_GameMaster.GetInstance().LoadLevel(mLevelID, mCurrentLevel);
        }
        entryCount++;
    }
}
