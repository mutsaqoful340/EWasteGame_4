using UnityEngine;

public class GP_PauseMenu : MonoBehaviour
{
    [Header("Animation Settings")]
    public string inAnimationName = "LaptopOpen";
    public string outAnimationName = "LaptopClose";
    private bool _isPaused;

    [Header("Object Control")]
    public GameObject targetObject; // Assign your object in Inspector (starts enabled)

    [Header("Audio Settings")]
    public AudioClip pauseSound;
    public AudioClip unpauseSound;

    private void Start()
    {
        // Initialize: Object starts ENABLED, game UNPAUSED
        _isPaused = false;

        if (targetObject != null)
        {
            targetObject.SetActive(true); // Force enable on start
            Debug.Log($"Initialized: Object starts enabled");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;

        // 1. Toggle object immediately
        if (targetObject != null)
        {
            targetObject.SetActive(_isPaused); // Will disable on first ESC, enable on second
            Debug.Log($"Toggled object to: {_isPaused}");
        }

        // 2. Handle other effects
        HandlePauseEffects();
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