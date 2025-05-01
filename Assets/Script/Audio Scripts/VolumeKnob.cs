using UnityEngine;
using UnityEngine.Audio;

public class VolumeKnob : MonoBehaviour
{
    public AudioMixer myMixer;

    public VolumeType volumeType = VolumeType.BGM;  // Choose via dropdown!

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

    private string ExposedParamName => volumeType.ToString(); // Auto map enum to string

    void Start()
    {
        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat(ExposedParamName + "Volume", 0.5f);
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

        // Hover logic
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
                CursorManager.Instance.StartDragging();
                isDragging = true;
                lastMousePos = Input.mousePosition;
            }
        }

        // While dragging
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float rotationAmount = delta.x * 0.2f;
            currentRotation += rotationAmount;
            currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation);

            ApplyRotation(currentRotation);

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
                CursorManager.Instance.StopDragging();

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
        myMixer.SetFloat(ExposedParamName, Mathf.Log10(volume01) * 20f);
        PlayerPrefs.SetFloat(ExposedParamName + "Volume", volume01);
    }
}

public enum VolumeType
{
    BGM,
    SFX
}
