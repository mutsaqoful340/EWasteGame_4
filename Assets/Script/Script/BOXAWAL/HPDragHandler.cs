using UnityEngine;

public class HPDragHandler : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        // Lepaskan dari parent (Box)
        transform.SetParent(null);
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 5f; // Jarak dari kamera, sesuaikan kalau perlu
        return cam.ScreenToWorldPoint(mousePoint);
    }
}
