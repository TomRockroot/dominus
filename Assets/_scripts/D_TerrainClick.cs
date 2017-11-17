using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class D_TerrainClick : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Terrain CLICK at " + eventData.pointerCurrentRaycast.worldPosition);
        D_PlayerControl controller = D_GameMaster.GetInstance().GetCurrentController();
        if (controller as D_PlayerControlClick)
        {
            Debug.Log("WHOOP WHOOP!");
            (controller as D_PlayerControlClick).SetDestination(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Terrain CLICK at " + eventData.pointerCurrentRaycast.worldPosition);
        D_PlayerControl controller = D_GameMaster.GetInstance().GetCurrentController();
        if (controller as D_PlayerControlClick)
        {
            Debug.Log("WHOOP WHOOP!");
            (controller as D_PlayerControlClick).SetDestination(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

}
