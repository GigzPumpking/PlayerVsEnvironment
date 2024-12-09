using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DragAndDrop[] DragAndDrops = GetComponentsInChildren<DragAndDrop>();
        foreach(DragAndDrop drageAndDrop in DragAndDrops)
        {
            drageAndDrop.canvas = canvas;
        }
    }
}
