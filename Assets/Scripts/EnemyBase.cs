using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    DragAndDrop DragObject;
    public bool justReleased = false;
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
                Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
                tmpRigidbody2D.linearVelocity = Vector2.zero;
                tmpRigidbody2D.position = DragObject.targetPos;
            }
            else
            {
                if (justReleased)
                {
                    Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
                    tmpRigidbody2D.linearVelocity = DragObject.dragVelocity * 500f;
                    print(DragObject.dragVelocity * 500f);
                }
                justReleased = false;
            }
        }
    }
}
