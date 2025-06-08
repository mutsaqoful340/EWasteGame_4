using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    public Animator animator;
    public Audio_Manager audioManager;

    [Header("Animation Settings")]
    public string animationName; // 👈 You can set this in Inspector now!

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
                    PlayCameraAnimation();  // ✅ Call the function
                }
            }
        }
    }

    // ✅ This is the function definition, outside of Update
    private void PlayCameraAnimation()
    {
        if (animator != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            animator.Play(animationName);
        }
        else
        {
            Debug.LogWarning("No Animator found on the Camera!");
        }
    }
}
