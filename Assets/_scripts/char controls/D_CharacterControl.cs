using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CharacterControl : MonoBehaviour {

    public Vector3 mMoveVector = Vector3.zero;
    public bool bMovementOverride = false;

    public D_Character mCharacter;

    protected D_GameMaster GAME_MASTER;

    void Start()
    {
        Initialize();
        GAME_MASTER = D_GameMaster.GetInstance();
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

    protected void Update ()
    {
        if(!bMovementOverride) mMoveVector = GetMoveVector();

        if (mMoveVector.magnitude > 0.01f)
        {
            transform.position += mMoveVector * mCharacter.GetPace() * Time.deltaTime * GAME_MASTER.GetGameMoveSpeed();
        }
	}

    public virtual Vector3 GetMoveVector()
    {
        return Vector3.zero;
    }

    public void OverrideMovement(bool val)
    {
        bMovementOverride = val;
    }
}
