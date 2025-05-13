using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    private float currentRotation = 0f;
    private bool isDragging = false;
    private bool isHovering = false;
    private Vector3 lastMousePos;
    void Start()
    {

    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hitThisKnob = false;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                hitThisKnob = true;
            }
        }

        // Hover logic
        if (!isDragging)
        {
            if (hitThisKnob)
            {
                if (!isHovering)
                {
                    isHovering = true;
                    CursorManager.Instance.SetGrabableCursor();
                }
            }
            else
            {
                if (isHovering)
                {
                    isHovering = false;
                    CursorManager.Instance.SetDefaultCursor();
                }
            }
        }

        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            if (hitThisKnob)
            {
                CursorManager.Instance.StartDragging();
                isDragging = true;
                lastMousePos = Input.mousePosition;
            }
        }

        // While dragging
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
        }

        // Release
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                CursorManager.Instance.StopDragging();

                if (isHovering)
                {
                    CursorManager.Instance.SetGrabableCursor();
                }
                else
                {
                    CursorManager.Instance.SetDefaultCursor();
                }
            }
        }
    }
}

