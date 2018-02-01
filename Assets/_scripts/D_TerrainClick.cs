using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class D_TerrainClick : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public void OnDrag(PointerEventData eventData)
    {
      //  Debug.Log("Terrain CLICK at " + eventData.pointerCurrentRaycast.worldPosition);
        D_PlayerControl controller = D_GameMaster.GetInstance().GetCurrentController();
        if (controller as D_PlayerControlClick)
        {
            (controller as D_PlayerControlClick).SetDestination(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Terrain CLICK at " + eventData.pointerCurrentRaycast.worldPosition);
        D_PlayerControl controller = D_GameMaster.GetInstance().GetCurrentController();
        if (controller as D_PlayerControlClick)
        {
            (controller as D_PlayerControlClick).SetDestination(eventData.pointerCurrentRaycast.worldPosition);
            D_UI_InteractionWheel.GetInstance().HideInteractions();
            Camera.main.GetComponent<D_CameraFollow>().SetCameraState(D_StructsAndEnums.ECameraMode.CM_Follow, D_GameMaster.GetInstance().GetCurrentController().mCharacter.transform);
        }
    }

}
