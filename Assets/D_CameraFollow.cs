using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CameraFollow : MonoBehaviour {

    public float mFollowSpeed = 1.0f;
    public Vector3 mOffset;

    public Transform mTarget;

    void Start()
    {
        mOffset = transform.position - mTarget.position;
    }

	void Update ()
    {
		if(mTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, mTarget.position + mOffset, Time.deltaTime * mFollowSpeed);
        }
    }
}
