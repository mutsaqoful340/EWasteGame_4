using UnityEngine;
using UnityEngine.Audio;

public class VolumeKnob : MonoBehaviour
{
    public AudioMixer myMixer;
    public string exposedParamName = "BGM";
    public float minRotation = -90f;
    public float maxRotation = 90f;

    private float currentRotation = 0f;
    private bool isDragging = false;
    private Vector3 lastMousePos;

    private float minVolume = 0.0001f;
    private float maxVolume = 1f;

    // Cursors
    public Texture2D defaultCursor;
    public Texture2D grabCursor;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat(exposedParamName + "Volume", 0.5f);
        float startRotation = Mathf.Lerp(minRotation, maxRotation, savedVolume);
        currentRotation = startRotation;
        transform.localEulerAngles = new Vector3(0, currentRotation, 0);

        SetVolume(savedVolume);
    }

    void Update()
    {
        // Check if player clicks on this knob
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = false;
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto); // Switch back to default

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    Cursor.SetCursor(grabCursor, hotspot, CursorMode.Auto);
                    isDragging = true;
                    lastMousePos = Input.mousePosition;
                }
            }
        }

        // While dragging
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotationAmount = delta.x * 0.2f; // Drag horizontally
            currentRotation += rotationAmount;
            currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation);

            // Apply rotation
            transform.localEulerAngles = new Vector3(0, currentRotation, 0);

            // Map rotation to 0-1 volume
            float volume01 = Mathf.InverseLerp(minRotation, maxRotation, currentRotation);
            SetVolume(volume01);

            lastMousePos = Input.mousePosition;
        }

        // Release
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void SetVolume(float volume01)
    {
        volume01 = Mathf.Clamp(volume01, minVolume, maxVolume);
        myMixer.SetFloat(exposedParamName, Mathf.Log10(volume01) * 20f);
        PlayerPrefs.SetFloat(exposedParamName + "Volume", volume01);
    }
}
