using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        } else {
            if (GameManager.Instance != null && GameManager.Instance.GetPlayer() != null) {
                target = GameManager.Instance.GetPlayer().transform;
            }
        }
    }
}
