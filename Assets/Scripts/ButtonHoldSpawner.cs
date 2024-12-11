using UnityEngine;
using UnityEngine.EventSystems;


//ChatGPT was my friend in this.
public class ButtonHoldSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private EnvironmentObject environmentObject; // Reference to the environment object
    private GameObject objectPrefab; // Prefab to spawn and drag
    private Canvas canvas;          // Reference to the canvas
    private GameObject spawnedObject;                // Reference to the currently spawned object
    private RectTransform spawnedRectTransform;      // RectTransform of the spawned object
    private bool isDragging = false;                 // Flag to track dragging state
    private Rigidbody2D objectRigidbody2D;               // Rigidbody of the spawned object
    private MonoBehaviour enemyMovementScript;
    private BoxCollider2D objectCollider;


    [SerializeField] private LayerMask layerMask;    // Layer mask to filter raycasts

    private void Start()
    {
        // Get the environment object reference from the parent
        environmentObject = GetComponentInParent<EnvironmentObject>();

        objectPrefab = environmentObject.GetObjectPrefab();

        // Get the canvas reference
        canvas = GameManager.Instance.GetObjectCanvas();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (environmentObject.ActivateObjectCheck())
        {
            if (objectPrefab == null) {
                objectPrefab = environmentObject.GetObjectPrefab();
            }

            // Spawn the object at the cursor's position
            SpawnObjectAtCursor(eventData);
        }
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
        objectRigidbody2D = spawnedObject.GetComponent<Rigidbody2D>();
        enemyMovementScript = spawnedObject.GetComponent<MonoBehaviour>();
        objectCollider = spawnedObject.GetComponent<BoxCollider2D>();

        // Disable collisions while dragging
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
        if (objectRigidbody2D != null)
        {
            objectRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            
            objectRigidbody2D.angularVelocity = 0f;
        }
        DisableAllMonoScripts(spawnedObject);
        // Set the initial position to the cursor's position in canvas local space
        UpdateObjectPositionToCursor(eventData);

        // Enable dragging
        isDragging = true;

        GameManager.Instance.AppendEnemy(spawnedObject);
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
            if (spawnedRectTransform)
            {
                spawnedRectTransform.anchoredPosition = cursorPosition;
            }
            else
            {
                spawnedObject.transform.position = cursorPosition;
            }
        }
    }
    private void DisableAllMonoScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }
    }

    private void EnableAllMonoScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
    }
    private void PlaceObject()
    {
        if (objectRigidbody2D != null)
        {
            objectRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        EnableAllMonoScripts(spawnedObject);
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }
        // Stop dragging
        isDragging = false;
        spawnedObject = null;
        spawnedRectTransform = null;

        Debug.Log("Object placed.");
    }
}
