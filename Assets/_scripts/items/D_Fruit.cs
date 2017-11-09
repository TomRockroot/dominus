using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Fruit : D_Item
{
    protected Color   mOriginalColor;
    protected Vector3 mOriginalSize;

    public Color mRipeColor = Color.green;

    [Range(0f, 1f)]
    public float mGrowthRate    = 0.1f;
    public float mSize          = 0.0f;

    void Awake()
    {
        mOriginalColor = GetComponent<Renderer>().material.color;
        mOriginalSize = transform.localScale;
    }

    public float Grow(float multiplyer = 1f)
    {
        if (mSize < 1f)
        {
            mSize += mGrowthRate * multiplyer * Time.deltaTime;
            transform.localScale = mOriginalSize * Mathf.Clamp(mSize, 0f, 1f);
            GetComponent<Renderer>().material.color = Color.Lerp(mOriginalColor, mRipeColor, Mathf.Clamp(mSize, 0f, 1f));
        }

        return mSize;
    }


}
