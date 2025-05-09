using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextButton : MonoBehaviour
{
    public TextMeshProUGUI sisaUangText;
    public BoxPenyimpanan boxPenyimpanan;

    public Toggle makanToggle; // Referensi ke Toggle "Makan"
    public GameObject warningOverlay; // Referensi ke overlay warning
    public Button closeButton; // Referensi ke tombol Close

    void Start()
    {
        // Pastikan closeButton sudah terhubung dan tambahkan listener
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWarningOverlay);
        }
    }

    public void OnNextLevelButtonPressed()
    {
        // Cek apakah Makan dicentang
        if (!makanToggle.isOn)
        {
            // Tampilkan warning overlay jika Makan tidak dicentang
            warningOverlay.SetActive(true);
            return; // Jangan lanjutkan ke level berikutnya
        }

        // Terapkan pilihan dulu sebelum lanjut
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        // Ambil sisa uang dari UI
        int sisa = int.Parse(sisaUangText.text.Split(' ')[1].Replace("Rp", "").Replace(",", ""));

        // Simpan sisa uang ke PlayerPrefs
        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();

        // Ambil index scene sekarang
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Cek apakah next scene masih dalam daftar
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Semua level sudah selesai!");
            // Kamu bisa tambahkan scene khusus seperti "GameSelesai" kalau mau
        }
    }

    // Menutup warning overlay
    public void CloseWarningOverlay()
    {
        warningOverlay.SetActive(false);

        // Langsung lanjut ke level berikutnya
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Semua level sudah selesai!");
            // Kamu bisa tambahkan scene khusus seperti "GameSelesai" kalau mau
        }
    }

}
