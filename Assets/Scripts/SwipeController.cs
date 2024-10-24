using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float directionX = 0f;
    public float directionY = 1f;
    public float magnitudeTreshold = 100f;

    public void OnEndDrag(PointerEventData eventData)
    {
        directionX = 0;
        directionY = 0;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition);
        if (dragVectorDirection.normalized.x > 0.5 || dragVectorDirection.normalized.x < -0.5)
        {
            directionX = dragVectorDirection.normalized.x;
        }
        if (dragVectorDirection.normalized.y < -0.5)
        {
            directionY = dragVectorDirection.normalized.y; ;
        }
    }
}
