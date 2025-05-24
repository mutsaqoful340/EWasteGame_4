using UnityEngine;

public class CameraZoomTransition : MonoBehaviour
{
    public Camera cameraToZoom;
    public float targetFOV = 10f;
    public float duration = 1f;

    private float originalFOV;
    private float timer;
    private bool isZooming = false;

    void Start()
    {
        if (cameraToZoom != null)
            originalFOV = cameraToZoom.fieldOfView;
    }

    public void StartZoom()
    {
        if (cameraToZoom == null) return;

        originalFOV = cameraToZoom.fieldOfView;
        timer = 0f;
        isZooming = true;
    }

    void Update()
    {
        if (!isZooming || cameraToZoom == null) return;

        timer += Time.deltaTime;
        float t = timer / duration;
        cameraToZoom.fieldOfView = Mathf.Lerp(originalFOV, targetFOV, t);

        if (t >= 1f)
        {
            cameraToZoom.fieldOfView = targetFOV;
            isZooming = false;
        }
    }
}
