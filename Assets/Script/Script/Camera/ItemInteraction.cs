using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public CameraFocus cameraFocus;  // Menggunakan script CameraFocus yang sudah dibuat

    void OnMouseDown()
    {
        // Ketika objek di klik, kamera akan zoom-in ke objek tersebut
        cameraFocus.ZoomIn();
    }
}
