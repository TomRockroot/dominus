using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using D_StructsAndEnums;

public class D_UI_FloatTextCanvas : MonoBehaviour
{
    public D_UI_FloatText pFloatText;
    public bool bDebugMessages = true;

    public void CreateFloatText(Vector3 worldPos, string content, EFloatText type, string contentDebug)
    {
        if(bDebugMessages)
        {
            CreateFloatText(worldPos, contentDebug, EFloatText.FT_Debug);
        }
        else
        {
            CreateFloatText(worldPos, content, type);
        }
    }

    public void CreateFloatText(Vector3 worldPos, string content, EFloatText type)
    {
        switch(type)
        {
            case EFloatText.FT_Debug:
                CreateFloatText(worldPos, content, 12, 2f, Vector3.up * 2f, Color.red);
                break;
            case EFloatText.FT_Speech:
                CreateFloatText(worldPos, content, 8, 2f, Vector3.up * 2f, Color.blue);
                break;
            case EFloatText.FT_Success:
                CreateFloatText(worldPos, content, 16, 1f, Vector3.up * 5f, Color.cyan);
                break;
            default:
                break;
        }
    }

    public void CreateFloatText(Vector3 worldPos, string content, int fontSize, float duration, Vector3 movement, Color color)
    {
        D_UI_FloatText floatText = Instantiate(pFloatText, transform);

        transform.position = worldPos;
        floatText.transform.localScale = Vector3.one;
        floatText.transform.localEulerAngles = Vector3.zero;
        floatText.transform.localPosition = Vector3.forward * -10f;

        floatText.mText.text = content;
        floatText.mText.fontSize = fontSize;
        floatText.mText.color = color;

        floatText.SetLifetime(duration);
        floatText.SetMovement(movement);
    }


    // ==== SINGLETON SHIT ====
    private static D_UI_FloatTextCanvas FLOAT_TEXT_CANVAS;

    public static D_UI_FloatTextCanvas GetInstance()
    {
        if (FLOAT_TEXT_CANVAS == null)
        {
            FLOAT_TEXT_CANVAS = GameObject.FindObjectOfType<D_UI_FloatTextCanvas>();
        }
        return FLOAT_TEXT_CANVAS;
    }
}
