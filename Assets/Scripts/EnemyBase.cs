using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    DragAndDrop DragObject;

    private bool justReleased = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DragObject != null)
        {
            if (DragObject.isBeingDragged)
            {
                justReleased = true;
                DraggedOnPoint(DragObject.targetPos);
            }
            else
            {
                if (justReleased)
                {
                    OnReleased(DragObject.dragVelocity);
                }
                justReleased = false;
            }
        }
    }

    void DraggedOnPoint(Vector2 targetPos)
    {
        Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        tmpRigidbody2D.linearVelocity = Vector2.zero;
        tmpRigidbody2D.MovePosition(targetPos);
    }

    void OnReleased(Vector2 releaseVelocity)
    {
        Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        tmpRigidbody2D.linearVelocity = releaseVelocity * 500f;
    }
}
