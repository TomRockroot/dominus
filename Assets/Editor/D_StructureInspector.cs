using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using D_StructsAndEnums;

[CustomEditor(typeof(D_Structure), true)]
public class D_StructureInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ((D_Structure)target).mRestrictionFlags = (EInteractionRestriction)EditorGUILayout.EnumMaskField("Restriction Flags", ((D_Structure)target).mRestrictionFlags);

        base.OnInspectorGUI();
    }
}