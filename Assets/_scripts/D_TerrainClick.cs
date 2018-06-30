using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class D_TerrainClick : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private A_Grid mGrid;

    public A_Grid GetGrid()
    {
        return mGrid;
    }

    public void SetGrid(A_Grid grid)
    {
        mGrid = grid;
    }

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

        if (controller as D_PlayerControlPath)
        {
            (controller as D_PlayerControlPath).FindPathTo( mGrid ,eventData.pointerCurrentRaycast.worldPosition);

            D_UI_InteractionWheel.GetInstance().HideInteractions();
            Camera.main.GetComponent<D_CameraFollow>().SetCameraState(D_StructsAndEnums.ECameraMode.CM_Follow, D_GameMaster.GetInstance().GetCurrentController().mCharacter.transform);
        }
    }

}
