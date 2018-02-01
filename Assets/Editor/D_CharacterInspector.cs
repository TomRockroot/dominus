using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using D_StructsAndEnums;

[CustomEditor(typeof(D_Character), true)]
public class D_CharacterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ((D_Character)target).mRestrictionFlags = (EInteractionRestriction)EditorGUILayout.EnumMaskField("Restriction Flags", ((D_Character)target).mRestrictionFlags);

        base.OnInspectorGUI();
    }
}
