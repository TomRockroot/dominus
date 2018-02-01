using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using D_StructsAndEnums;

[CustomEditor(typeof(D_GameMaster))]
public class D_GameMasterInspector : Editor
{
    

    public override void OnInspectorGUI()
    {
        ((D_GameMaster)target).mDebugFlags = (EDebugLevel)EditorGUILayout.EnumMaskField("Debug Flags", ((D_GameMaster)target).mDebugFlags);

        base.OnInspectorGUI();
    }
}
