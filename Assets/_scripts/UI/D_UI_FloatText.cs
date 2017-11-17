using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_UI_FloatText : MonoBehaviour {

    public Text mText;
    protected Vector3 mMove = Vector3.zero;

    void Start()
    {
        mText = GetComponent<Text>();
    }

    void Update()
    {
        transform.localPosition = transform.localPosition + mMove * Time.deltaTime;
    }

    public void SetLifetime(float duration)
    {
        Destroy(gameObject, duration);   
    }

    public void SetMovement(Vector3 movement)
    {
        mMove = movement;
    }
}
