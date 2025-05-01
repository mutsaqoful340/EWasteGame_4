using UnityEngine;
using UnityEngine.Audio;

public class VolumeKnob1 : MonoBehaviour
{
    public AudioMixer myMixer;
    public string exposedParamName = "SFX";
    public float minRotation = -90f;
    public float maxRotation = 90f;

    public enum RotationAxis { X, Y, Z }
    public RotationAxis rotationAxis = RotationAxis.Z; // Default to Z

    private float currentRotation = 0f;
    private bool isDragging = false;
    private Vector3 lastMousePos;

    private float minVolume = 0.0001f;
    private float maxVolume = 1f;

    void Start()
    {
        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat(exposedParamName + "Volume", 0.5f);
        float startRotation = Mathf.Lerp(minRotation, maxRotation, savedVolume);
        currentRotation = startRotation;

        ApplyRotation(currentRotation);
        SetVolume(savedVolume);
    }

    void Update()
    {
        // Check if player clicks on this knob
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = false;
            CursorManager.Instance.SetDefaultCursor(); // Switch back to default

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    CursorManager.Instance.SetGrabCursor(); // Switch to grab cursor
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

            ApplyRotation(currentRotation);

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

    private void ApplyRotation(float rotation)
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                transform.localEulerAngles = new Vector3(rotation, 0, 0);
                break;
            case RotationAxis.Y:
                transform.localEulerAngles = new Vector3(0, rotation, 0);
                break;
            case RotationAxis.Z:
                transform.localEulerAngles = new Vector3(0, 43.764f, rotation);
                break;
        }
    }

    private void SetVolume(float volume01)
    {
        volume01 = Mathf.Clamp(volume01, minVolume, maxVolume);
        myMixer.SetFloat(exposedParamName, Mathf.Log10(volume01) * 20f);
        PlayerPrefs.SetFloat(exposedParamName + "Volume", volume01);
    }
}
