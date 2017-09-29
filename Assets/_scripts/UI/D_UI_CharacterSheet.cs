using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_UI_CharacterSheet : MonoBehaviour {

    public D_UI_Inventory mInventoryUI;
    public D_UI_Needs mNeedsUI;


    // ==== SINGLETON SHIT ====
    private static D_UI_CharacterSheet CHARACTER_SHEET;

	public static D_UI_CharacterSheet GetInstance()
    {
        if(CHARACTER_SHEET == null)
        {
            CHARACTER_SHEET = GameObject.FindObjectOfType<D_UI_CharacterSheet>();
        }
        return CHARACTER_SHEET;
    }
}
