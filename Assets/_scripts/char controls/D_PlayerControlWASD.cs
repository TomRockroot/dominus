using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerControlWASD : D_CharacterControl {


    public override Vector3 GetMoveVector()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    public override void CheckMouseButton(int button = 0)
    {
        if (Input.GetMouseButtonDown(button))
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log("Clicked("+ button +") GO: " + hit.transform.name);
                if (hit.transform.GetComponent<D_ITargetable>() != null)
                {
                    switch (button)
                    {
                        case 0:
                            hit.transform.GetComponent<D_ITargetable>().InteractPrimary(this);
                            break;
                        case 1:
                            hit.transform.GetComponent<D_ITargetable>().InteractSecondary(this);
                            break;
                    }
                }
            }
        }
    }
}
