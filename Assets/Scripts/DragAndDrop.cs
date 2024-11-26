using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

//using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

//Multiple Handlers needed to allow Unity style dragging
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private bool isDraggable;//drag toggle

    private void Awake()
    {
        isDraggable = true;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();//used to block raycasts during the drag.  Needed if prevent blocking pointer events
        startPos = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            eventData.pointerDrag = null;
            return;
        }
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
        UnityEngine.Debug.Log("dragging");
        startPos = transform.position;
    }
    public void setDraggable(bool draggable)
    {
        isDraggable = draggable;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }

        //We set the globalMousePosition using the cursor location
        //followed by converting it into the world coordinates.
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform, eventData.position,
            eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
        transform.position = startPos;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            eventData.pointerDrag = null;
            //run this if for food to tell the player
            return;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            transform.position = startPos;
            return;
        }
    }
}

