using UnityEngine;

public class GP_PauseMenu : MonoBehaviour
{
    [Header("Animation Settings")]
    public string inAnimationName = "LaptopOpen";
    public string outAnimationName = "LaptopClose";
    private bool _isPaused;

    [Header("Audio Settings")]
    public AudioClip pauseSound;
    public AudioClip unpauseSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        HandlePauseEffects(); // Only handle animations/sounds
    }

    private void HandlePauseEffects()
    {
        // Animation
        string animationToPlay = _isPaused ? inAnimationName : outAnimationName;
        if (Camera_Manager.Instance != null && Camera_Manager.Instance.CurrentCameraAnimator != null)
        {
            Camera_Manager.Instance.CurrentCameraAnimator.Play(animationToPlay);
        }
        else
        {
            Debug.LogError("Camera_Manager or Animator missing!");
        }

        // Sound
        if (Audio_Manager.Instance != null)
        {
            AudioClip soundToPlay = _isPaused ? pauseSound : unpauseSound;
            AudioClip clipToUse = soundToPlay != null ? soundToPlay : Audio_Manager.Instance.buttonClick;
            Audio_Manager.Instance.PlaySFX(clipToUse);
        }
        else
        {
            Debug.LogError("Audio_Manager missing!");
        }
    }
}
