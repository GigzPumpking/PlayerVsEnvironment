using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnvironmentObject : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    private Image fill; // Black filter over the icon
    private TextMeshProUGUI cooldownText; // Text displaying the cooldown
    [SerializeField] private float cooldown = 5.0f; // The cooldown of the object
    [SerializeField] private float cooldownTimer = 0.0f; // The timer for the object cooldown
    [SerializeField] private bool isOnCooldown = false; // Indicates if the object is on cooldown

    [SerializeField] private bool debug = false; // Debug toggle for logs

    void Awake()
    {
        // Get the TextMeshProUGUI component from the child named "CooldownText"
        cooldownText = transform.Find("Cooldown").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        // Get the Image component from the child named "Fill"
        fill = transform.Find("Fill").GetComponent<Image>();

        if (fill != null)
        {
            fill.enabled = false;
        }

        // Set the cooldown text to an empty string
        cooldownText.text = "";
    }

    void Update()
    {
        if (isOnCooldown)
        {
            // Update the cooldown timer
            cooldownTimer -= Time.deltaTime;

            // Update the cooldown text
            cooldownText.text = $"{cooldownTimer:0.0}";

            // Check if the cooldown timer has reached zero
            if (cooldownTimer <= 0.0f)
            {
                // Reset the cooldown timer
                cooldownTimer = 0.0f;

                // Indicate that the object is no longer on cooldown
                isOnCooldown = false;

                // Hide the black filter
                if (fill != null)
                {
                    fill.enabled = false;
                }

                // Set the cooldown text to the key code
                cooldownText.text = "";
            }
        }
    }

    public void ActivateObject()
    {
        // Check if the object is not on cooldown
        if (!isOnCooldown)
        {
            if (debug)
            {
                Debug.Log($"{objectPrefab.name} activated.");
            }

            bool success = objectPrefab != null ? true : false;

            if (!success)
            {
                if (debug)
                {
                    Debug.Log($"{objectPrefab.name} failed to apply.");
                }

                return;
            }

            // Start the cooldown timer
            cooldownTimer = cooldown;

            // Indicate that the object is on cooldown
            isOnCooldown = true;

            // Show the black filter
            if (fill != null)
            {
                fill.enabled = true;
            }
        }
    }

    public bool ActivateObjectCheck()
    {
        // Check if the object is not on cooldown
        if (!isOnCooldown)
        {
            if (debug)
            {
                Debug.Log($"{objectPrefab.name} activated.");
            }

            bool success = objectPrefab != null ? true : false;

            if (!success)
            {
                if (debug)
                {
                    Debug.Log($"{objectPrefab.name} failed to apply.");
                }

                return false;
            }

            // Start the cooldown timer
            cooldownTimer = cooldown;

            // Indicate that the object is on cooldown
            isOnCooldown = true;

            // Show the black filter
            if (fill != null)
            {
                fill.enabled = true;
            }

            return true;
        } else
        {
            return false;
        }
    }

    private bool ApplyObject()
    {
        if (objectPrefab == null)
        {
            return false;
        }

        // Instantiate the object prefab on the cursor position relative to the camera as a child of the object canvas

        // The position should not consider the z-axis
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0.0f;
        GameObject obj = Instantiate(objectPrefab, position, Quaternion.identity, GameManager.Instance.GetObjectCanvas().transform);

        if (obj == null)
        {
            return false;
        }
        
        return true;
    }

    public GameObject GetObjectPrefab()
    {
        return objectPrefab;
    }
}
