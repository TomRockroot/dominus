using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_CameraFollow : MonoBehaviour {

    public float mFollowSpeed = 1.0f;
    public float mExtDistance = 2.0f;
    public Vector3 mOffset;

    public float mExtHeightMax = 2.0f;
    public float mHeightSpeedUp = 2.0f;
    public float mHeightSpeedDown = 4.0f;
    public float mTimeSpentMinimum = 1.0f;

    private float mTimeSpentMoving = 0f;
    private float mHeightOffset = 0f;

    public Transform mTarget;
    private D_CharacterControl mController;


    private ECameraMode mState = ECameraMode.CM_Follow;

    public void SetCameraState(ECameraMode state, Transform target)
    {
        Debug.Log("Camera: " + mState + " ~> " + state);

        if (target != null)
        {
            mTarget = target;
        }

        switch (state)
        {
            case ECameraMode.CM_Focus:

                break;
            case ECameraMode.CM_Follow:
                Debug.Log("Camera: Following " + target.name);
                break;
            case ECameraMode.CM_Freefly:
                break;
            case ECameraMode.CM_Rail:
                break;
        }

        mState = state;
    }

    void Start()
    {
        if(mTarget == null)
        {
            Debug.LogError("Camera has no mTarget!");
            Debug.Break();
        }
        
        mController = mTarget.GetComponent<D_CharacterControl>();
    }

	void Update ()
    {
        switch (mState)
        {
            case ECameraMode.CM_Follow:
                if (mController.mMoveVector.magnitude > 0.7f)
                {
                    mTimeSpentMoving += Time.deltaTime * mHeightSpeedUp;

                    if (mTimeSpentMoving > mTimeSpentMinimum)
                    {
                        mHeightOffset = Mathf.Clamp((mTimeSpentMoving - mTimeSpentMinimum), 0f, mExtHeightMax);
                    }
                }
                else
                {
                    mTimeSpentMoving = 0f;
                    mHeightOffset = 0f;
                }


                if (! GetComponent<Camera>().orthographic)
                {
                    transform.position = Vector3.Lerp(transform.position, mTarget.position + mOffset + transform.forward * (-mHeightOffset) + mController.mMoveVector * mExtDistance, Time.deltaTime * mFollowSpeed);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, mTarget.position + mOffset + mController.mMoveVector * mExtDistance, Time.deltaTime * mFollowSpeed);
                    GetComponent<Camera>().orthographicSize = 3f + mHeightOffset;
                }
                break;

            case ECameraMode.CM_Focus:
                transform.position = Vector3.Lerp(transform.position, mTarget.position + mOffset, Time.deltaTime * mFollowSpeed);
                D_UI_InteractionWheel.GetInstance().SetWheelPosition(GetComponent<Camera>().WorldToScreenPoint(mTarget.position));
                break;

            case ECameraMode.CM_Freefly:
                break;

            case ECameraMode.CM_Rail:
                break;
        }   
    }
}
