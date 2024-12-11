using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    DragAndDrop DragObject;

    private bool justReleased = false;

    private bool isBeingDragged = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if (DragObject != null)
        {
            if (DragObject.isBeingDragged)
            {
                justReleased = true;
                DraggedOnPoint(DragObject.targetPos);
                isBeingDragged = true;
            }
            else
            {
                if (justReleased)
                {
                    OnReleased(DragObject.dragVelocity);
                }
                justReleased = false;
                isBeingDragged = false;
            }
        }

        if (isBeingDragged)
        {
            // Turn off collision
            if (gameObject.GetComponent<Collider2D>().enabled)
                gameObject.GetComponent<Collider2D>().enabled = false;
        } else {
            // Turn on collision
            if (!gameObject.GetComponent<Collider2D>().enabled)
                gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    protected void DraggedOnPoint(Vector2 targetPos)
    {
        Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        tmpRigidbody2D.linearVelocity = Vector2.zero;
        tmpRigidbody2D.MovePosition(targetPos);
    }

    protected void OnReleased(Vector2 releaseVelocity)
    {
        Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        tmpRigidbody2D.linearVelocity = releaseVelocity * 500f;
    }

    public bool IsBeingDragged()
    {
        return isBeingDragged;
    }
}
