using UnityEngine;
using UnityEngine.Audio;

public class VolumeKnob : MonoBehaviour
{
    public AudioMixer myMixer;
    public string exposedParamName = "BGM";
    public float minRotation = -90f;
    public float maxRotation = 90f;

    public enum RotationAxis { X, Y, Z }
    public RotationAxis rotationAxis = RotationAxis.Z; // Default to Z

    private float currentRotation = 0f;
    private bool isDragging = false;
    private bool isHovering = false;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hitThisKnob = false;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                hitThisKnob = true;
            }
        }

        // Hover logic (only if not dragging)
        if (!isDragging)
        {
            if (hitThisKnob)
            {
                if (!isHovering)
                {
                    isHovering = true;
                    CursorManager.Instance.SetGrabableCursor();
                }
            }
            else
            {
                if (isHovering)
                {
                    isHovering = false;
                    CursorManager.Instance.SetDefaultCursor();
                }
            }
        }

        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            if (hitThisKnob)
            {
                CursorManager.Instance.StartDragging(); // Global drag flag
                isDragging = true;
                lastMousePos = Input.mousePosition;
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
            if (isDragging)
            {
                isDragging = false;
                CursorManager.Instance.StopDragging(); // End global drag

                // After release: if still hovering, show Grabable cursor
                if (isHovering)
                {
                    CursorManager.Instance.SetGrabableCursor();
                }
                else
                {
                    CursorManager.Instance.SetDefaultCursor();
                }
            }
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
