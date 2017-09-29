using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CharacterControl : MonoBehaviour {

    public Vector3 mMoveVector = Vector3.zero;

    public D_Character mCharacter;

    void Start()
    {
        Initialize();
        
    }

    protected virtual void Initialize()
    {
        Debug.Log("Initializing CharacterControl: " + name);

        mCharacter = GetComponent<D_Character>();
        if (mCharacter == null)
        {
            Debug.LogError("No Character on " + name);
            Debug.Break();
        }
    }

	void Update ()
    {
        mMoveVector = GetMoveVector();
        CheckMouseButton(0);
        CheckMouseButton(1);


        if (mMoveVector.magnitude > 0.01f)
        {
            transform.position += mMoveVector * mCharacter.GetPace() * Time.deltaTime;
        }
	}

    public virtual Vector3 GetMoveVector()
    {
        return Vector3.zero;
    }

    public virtual void CheckMouseButton(int button = 0)
    {

    }
}
