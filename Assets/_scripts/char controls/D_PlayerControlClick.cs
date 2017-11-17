using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlayerControlClick : D_PlayerControl
{
    public Vector3 mDestination;

    public void SetDestination(Vector3 destination)
    {
        mDestination = destination;
    }

    protected override void Initialize()
    {
        mDestination = transform.position;

        base.Initialize();
    }

    public override Vector3 GetMoveVector()
    {
        Vector3 characterToDestination = Vector3.Scale(mDestination, new Vector3(1f, 0f, 1f)) - Vector3.Scale(transform.position, new Vector3(1f, 0f, 1f));
        if(characterToDestination.magnitude > 1.0f)
        {
            characterToDestination.Normalize();
        }

        return characterToDestination;
    }
}
