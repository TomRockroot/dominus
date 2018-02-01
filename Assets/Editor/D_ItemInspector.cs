using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using D_StructsAndEnums;

[CustomEditor(typeof(D_Item), true)]
public class D_ItemInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ((D_Item)target).mRestrictionFlags = (EInteractionRestriction)EditorGUILayout.EnumMaskField("Restriction Flags", ((D_Item)target).mRestrictionFlags);

        base.OnInspectorGUI();
    }
}