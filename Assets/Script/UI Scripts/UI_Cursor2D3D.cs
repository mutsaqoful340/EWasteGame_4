using UnityEngine;

public class UI_CUrsor2D3D : MonoBehaviour
{
    private bool isDragging = false;
    private bool isHovering = false;
    private Vector3 lastMousePos;

    void Update()
    {
        bool hitThisObject = false;

        // Check for 3D hit
        Ray ray3D = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray3D, out RaycastHit hit3D))
        {
            if (hit3D.transform == transform)
            {
                hitThisObject = true;
            }
        }

        // Check for 2D hit (if not hit in 3D)
        if (!hitThisObject)
        {
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit2D.collider != null && hit2D.transform == transform)
            {
                hitThisObject = true;
            }
        }

        // Hover logic
        if (!isDragging)
        {
            if (hitThisObject && !isHovering)
            {
                isHovering = true;
                CursorManager.Instance.SetGrabableCursor();
            }
            else if (!hitThisObject && isHovering)
            {
                isHovering = false;
                CursorManager.Instance.SetDefaultCursor();
            }
        }

        // Release
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            CursorManager.Instance.StopDragging();

            if (isHovering)
                CursorManager.Instance.SetGrabableCursor();
            else
                CursorManager.Instance.SetDefaultCursor();
        }
    }
}
