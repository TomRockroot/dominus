using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class D_PlayerControlWASD : D_PlayerControl
{

    public override Vector3 GetMoveVector()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(move.magnitude > 1f)
        {
            move.Normalize();
        }
        return move;
    }  
}
