using UnityEngine;
using System.Collections;

public class CameraFocus : MonoBehaviour
{
    public Camera playerCamera;
    public float zoomedFOV = 50f; // FOV saat zoom-in
    public float defaultFOV = 60f; // FOV normal
    public float zoomSpeed = 5f; // Kecepatan perubahan FOV

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.fieldOfView = defaultFOV; // Set FOV ke default saat permainan dimulai
    }

    public void ZoomIn()
    {
        StartCoroutine(ZoomCamera(zoomedFOV));
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomCamera(defaultFOV));
    }

    private IEnumerator ZoomCamera(float targetFOV)
    {
        float startFOV = playerCamera.fieldOfView;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * zoomSpeed;
            playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
            yield return null;
        }
    }
}
