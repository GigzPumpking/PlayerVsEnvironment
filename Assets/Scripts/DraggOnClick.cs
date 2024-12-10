using UnityEngine;
using UnityEngine.EventSystems;


//ChatGPT was my friend in this.
public class ButtonHoldSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject objectPrefab; // Prefab to spawn and drag
    [SerializeField] private Canvas canvas;          // Reference to the canvas
    private GameObject spawnedObject;                // Reference to the currently spawned object
    private RectTransform spawnedRectTransform;      // RectTransform of the spawned object
    private bool isDragging = false;                 // Flag to track dragging state

    public void OnPointerDown(PointerEventData eventData)
    {
        // Spawn the object at the cursor's position
        SpawnObjectAtCursor(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop dragging when the pointer is released
        PlaceObject();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Drag the spawned object
        if (isDragging && spawnedRectTransform != null)
        {
            UpdateObjectPositionToCursor(eventData);
        }
    }

    private void SpawnObjectAtCursor(PointerEventData eventData)
    {
        // Instantiate the object prefab
        spawnedObject = Instantiate(objectPrefab, canvas.transform);
        spawnedRectTransform = spawnedObject.GetComponent<RectTransform>();

        // Set the initial position to the cursor's position in canvas local space
        UpdateObjectPositionToCursor(eventData);

        // Enable dragging
        isDragging = true;
    }

    private void UpdateObjectPositionToCursor(PointerEventData eventData)
    {
        Vector2 cursorPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out cursorPosition))
        {
            // Update the object's anchored position to match the cursor
            spawnedRectTransform.anchoredPosition = cursorPosition;
        }
    }

    private void PlaceObject()
    {
        // Stop dragging
        isDragging = false;
        spawnedObject = null;
        spawnedRectTransform = null;

        Debug.Log("Object placed.");
    }
}
