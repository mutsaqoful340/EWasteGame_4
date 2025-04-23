using UnityEngine;

public class PuzzleComplete : MonoBehaviour
{
    public GameObject puzzlePanel;
    public CameraFocus cameraFocus;

    public void OnPuzzleCompleted()
    {
        puzzlePanel.SetActive(false);  // Tutup panel puzzle
        cameraFocus.ZoomOut();         // Kembali ke view normal
        // Kamu bisa aktifkan interaksi memilah barang di sini
    }
}
