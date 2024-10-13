using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount != 0) return;

        GameObject l_droppedObject = eventData.pointerDrag;
        DragNDrop2D l_draggableObject = l_droppedObject.GetComponent<DragNDrop2D>();
        l_draggableObject.SetParentAfterDrag(transform);
    }
}