using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CameraFollow : MonoBehaviour {

    public float mFollowSpeed = 1.0f;
    public float mExtDistance = 2.0f;
    public Vector3 mOffset;

    public Transform mTarget;
    private D_CharacterControl mController;
    private bool bUseController;

    void Start()
    {
        if(mTarget == null)
        {
            Debug.LogError("Camera has no mTarget!");
            Debug.Break();
        }

        mOffset = transform.position - mTarget.position;
        mController = mTarget.GetComponent<D_CharacterControl>();
        bUseController = mController != null;
    }

	void Update ()
    {
        if(bUseController)
        {
            transform.position = Vector3.Lerp(transform.position, mTarget.position + mOffset + mController.mMoveVector * mExtDistance, Time.deltaTime * mFollowSpeed);
        }
        else
        { 
            transform.position = Vector3.Lerp(transform.position, mTarget.position + mOffset, Time.deltaTime * mFollowSpeed);
        }
    }
}
