using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject miniGameCamera;
    public GameObject pcbOnTable;
    public GameObject panelMiniGameUI;

    // Tambah referensi UI gameplay utama yang mau disembunyikan sementara
    public GameObject textTimer;
    public GameObject textTabungan;
    public GameObject textUpah;

    public void StartMiniGame()
    {
        mainCamera.SetActive(false);
        miniGameCamera.SetActive(true);
        pcbOnTable.SetActive(true);
        panelMiniGameUI.SetActive(true);

        // Hide UI gameplay utama sementara
        textTimer.SetActive(false);
        textTabungan.SetActive(false);
        textUpah.SetActive(false);
    }

    public void EndMiniGame()
    {
        miniGameCamera.SetActive(false);
        mainCamera.SetActive(true);
        pcbOnTable.SetActive(false);
        panelMiniGameUI.SetActive(false);

        // Show UI gameplay utama kembali
        textTimer.SetActive(true);
        textTabungan.SetActive(true);
        textUpah.SetActive(true);
    }
}
