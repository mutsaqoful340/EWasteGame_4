using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    // Reference to your camera animator
    public Animator cameraAnimator;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                // Click
                if (Input.GetMouseButtonDown(0))
                {
                    OnButtonClicked();
                }
            }
        }
    }

    private void OnButtonClicked()
    {
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
