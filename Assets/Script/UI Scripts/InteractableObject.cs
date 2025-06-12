using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    [Header("Animation Settings")]
    public string animationName; // Set in Inspector

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayCameraAnimation();
            }
        }
    }

    private void PlayCameraAnimation()
    {
        Audio_Manager.Instance?.PlaySFX(Audio_Manager.Instance.buttonClick);

        if(Camera_Manager.Instance?.CurrentCameraAnimator == null)
        {
            Debug.LogWarning("Animator issing");
            return;
        }
        Camera_Manager.Instance?.CurrentCameraAnimator?.Play(animationName);
    }
}