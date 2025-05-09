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

    private bool warningSudahMuncul = false; // Flag untuk status warning

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWarningOverlay);
        }
    }

    public void OnNextLevelButtonPressed()
    {
        // Jika toggle makan belum dicentang dan warning belum pernah muncul
        if (!makanToggle.isOn && !warningSudahMuncul)
        {
            warningOverlay.SetActive(true);
            warningSudahMuncul = true; // Tandai bahwa warning sudah pernah muncul
            return;
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

        // Pindah ke scene berikutnya
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Semua level sudah selesai!");
        }
    }

    public void CloseWarningOverlay()
    {
        warningOverlay.SetActive(false);
        // Tidak pindah scene di sini. User harus klik Next lagi.
    }
}
