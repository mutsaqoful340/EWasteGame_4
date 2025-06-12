using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    public static Camera_Manager Instance;  // Global access point
    public Animator CurrentCameraAnimator { get; private set; }

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
            UpdateCameraReference();  // Cache initial camera
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
    }

    // Call this when cameras change (e.g., scene load or camera switch)
    public void UpdateCameraReference()
    {
        if (Camera.main != null)
        {
            CurrentCameraAnimator = Camera.main.GetComponent<Animator>();
            if (CurrentCameraAnimator == null)
                Debug.LogWarning("Main Camera has no Animator component!");
        }
        else
        {
            Debug.LogWarning("No Main Camera found in this scene!");
        }
    }
}