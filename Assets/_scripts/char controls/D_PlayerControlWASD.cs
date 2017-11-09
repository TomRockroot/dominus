using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerControlWASD : D_CharacterControl {

    public D_Interaction mPreparedInteraction;


    public override Vector3 GetMoveVector()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(move.magnitude > 1f)
        {
            move.Normalize();
        }
        return move;
    }

    public override void CheckMouseButton(int button = 0)
    {
        // ToDo: 
        // If(mPreparedInteraction == null) Open InteractionWheel ... Go from there, Tom !!
        


        if (Input.GetMouseButtonDown(button))
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log("Clicked("+ button +") GO: " + hit.transform.name);

                D_ITargetable target = hit.transform.GetComponent<D_ITargetable>();
                D_UI_Interaction uiInteraction = hit.transform.GetComponent<D_UI_Interaction>();
                if(uiInteraction != null)
                {
                    D_UI_InteractionWheel.GetInstance().HideInteractions();
                    mPreparedInteraction = Instantiate( uiInteraction.mContainedInteraction );
                    StartCoroutine("DoInteraction");
                }
                else if (target != null)
                {
                    switch (button)
                    {
                        case 0:
                            Debug.Log("SHOW ME THE MORTY!");
                            break;
                        case 1:
                            Debug.Log("WABBA LABBA DUB DUB!");
                            break;
                    }

                    D_UI_InteractionWheel.GetInstance().ShowInteractions(target);
                }
            }
        }
    }

    IEnumerator DoInteraction()
    {
        Debug.Log(name + " is preparing to execute " + mPreparedInteraction.name);
        while (!mPreparedInteraction.ExecuteInteraction())
        {
            Debug.Log(name + " is executing " + mPreparedInteraction.name);
            yield return new WaitForEndOfFrame();
        }
    }
}
