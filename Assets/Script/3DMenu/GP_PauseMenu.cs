using UnityEngine;

public class GP_PauseMenu : MonoBehaviour
{
    [Header("Animation Settings")]
    public string inAnimationName = "LaptopOpen";
    public string outAnimationName = "LaptopClose";
    private bool _isPaused; // Made private to prevent external interference

    [Header("Object Control")]
    public GameObject targetObject;

    [Header("Audio Settings")]
    public AudioClip pauseSound;
    public AudioClip unpauseSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle state FIRST
            _isPaused = !_isPaused;

            // 1. Immediately handle object toggle
            if (targetObject != null)
            {
                targetObject.SetActive(_isPaused);
                Debug.Log($"Toggled object: {targetObject.name} to {_isPaused}");
            }

            // 2. Then handle visual/audio feedback
            HandlePauseEffects();
        }
    }

    private void HandlePauseEffects()
    {
        // Animation
        string animationToPlay = _isPaused ? inAnimationName : outAnimationName;
        Camera_Manager.Instance?.CurrentCameraAnimator?.Play(animationToPlay);

        // Sound
        AudioClip soundToPlay = _isPaused ? pauseSound : unpauseSound;
        Audio_Manager.Instance?.PlaySFX(soundToPlay != null ? soundToPlay : Audio_Manager.Instance.buttonClick);
    }
}