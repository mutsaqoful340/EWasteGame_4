using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    public GameObject puzzlePanel; // UI puzzle
    public CameraFocus cameraFocus;

    void OnMouseDown()
    {
        cameraFocus.ZoomIn();          // Zoom ke komputer
        puzzlePanel.SetActive(true);   // Tampilkan puzzle UI
    }
}
