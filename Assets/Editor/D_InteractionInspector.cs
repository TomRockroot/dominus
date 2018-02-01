using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using D_StructsAndEnums;

[CustomEditor(typeof(D_Interaction), true)]
public class D_InteractionInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ((D_Interaction)target).mRestrictionFlags = (EInteractionRestriction)EditorGUILayout.EnumMaskField("Restriction Flags", ((D_Interaction)target).mRestrictionFlags);

        base.OnInspectorGUI();
    }
}
