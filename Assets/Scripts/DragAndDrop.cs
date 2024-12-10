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
    [SerializeField] public Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPos;
    private bool isDraggable;//drag toggle
    Vector3 DragOffset;
    public bool isBeingDragged = false;
    public Vector2 targetPos;
    public Vector2 lastTargetPos;
    public Vector2 dragVelocity;

    private void SetTargetPos(Vector3 pos)
    {
        lastTargetPos = targetPos;
        targetPos = pos;
        dragVelocity = targetPos - lastTargetPos;
    }

    private void Awake()
    {
        isDraggable = true;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();//used to block raycasts during the drag.  Needed if prevent blocking pointer events
        startPos = transform.position;
        targetPos = transform.position;
    }

    private void OnEnable()
    {
        if (canvas == null && GameManager.Instance != null) {
            canvas = GameManager.Instance.GetObjectCanvas();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeingDragged = true;
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
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform, eventData.position,
            eventData.pressEventCamera, out globalMousePos))
        {
            DragOffset = rectTransform.position - globalMousePos;
        }
    }
    public void setDraggable(bool draggable)
    {
        isDraggable = draggable;
    }
    public void OnDrag(PointerEventData eventData)
    {
        isBeingDragged = true;
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
            rectTransform.position = globalMousePos + DragOffset;
            SetTargetPos(globalMousePos + DragOffset);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isBeingDragged = false;
        targetPos = transform.position;
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
        //transform.position = startPos;

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
        targetPos = transform.position;
        isBeingDragged = false;
        if (!isDraggable)
        {
            //transform.position = startPos;
            return;
        }
    }
}

