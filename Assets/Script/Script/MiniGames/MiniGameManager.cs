using UnityEngine;
using System.Collections;

public class MiniGameManager : MonoBehaviour
{

    public CameraZoomTransition zoomTransition;
    public GameObject mainCamera;
    public GameObject miniGameCamera;
    public GameObject pcbOnTable;

    public GameObject miniGameUIPanel;   // Panel UI mini game
    public GameObject gameplayUI;         // UI gameplay utama (timer, tabungan, dll)

    public void StartMiniGame()
    {
        gameplayUI.SetActive(false);
        mainCamera.SetActive(false);
        miniGameCamera.SetActive(true);

        pcbOnTable.SetActive(true);
        miniGameUIPanel.SetActive(true);

        StartCoroutine(StartZoomDelayed());
    }

    IEnumerator StartZoomDelayed()
    {
        yield return new WaitForSeconds(0.1f); // beri waktu 1 frame setelah kamera aktif
        if (zoomTransition != null)
            zoomTransition.StartZoom();
    }

    public void EndMiniGame()
    {
        miniGameCamera.SetActive(false);
        mainCamera.SetActive(true);
        pcbOnTable.SetActive(false);

        miniGameUIPanel.SetActive(false);
        gameplayUI.SetActive(true);
    }
}

