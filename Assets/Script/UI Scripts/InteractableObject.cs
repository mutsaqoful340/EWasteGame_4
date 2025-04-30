using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Referencing Animator
    public Animator cameraAnimator;

    private bool isHovering = false;

    void Update()
    {
        // Raycast from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                // Start hover
                if (!isHovering)
                {
                    isHovering = true;
                    CursorManager.Instance.SetGrabableCursor(); // Show grabable cursor
                }

                // Check click
                if (Input.GetMouseButtonDown(0))
                {
                    OnButtonClicked();
                }
            }
            else
            {
                StopHover();
            }
        }
        else
        {
            StopHover();
        }
    }

    private void StopHover()
    {
        if (isHovering)
        {
            isHovering = false;
            CursorManager.Instance.SetDefaultCursor(); // Back to default cursor
        }
    }

    private void OnButtonClicked()
    {
        // Your action here
        PlayCameraAnimation("Radio");
    }

    public void PlayCameraAnimation(string triggerName)
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("No Animator found on the Camera!");
        }
    }
}
