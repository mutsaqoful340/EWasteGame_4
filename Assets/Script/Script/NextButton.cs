using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextButton : MonoBehaviour
{
    public TextMeshProUGUI sisaUangText;
    public BoxPenyimpanan boxPenyimpanan;

    public Toggle makanToggle;
    public GameObject warningOverlay;
    public Button closeButton;

    private bool warningSudahMuncul = false;
    public string endingSceneName = "EndingScene"; // Ganti dengan nama scene yang sesuai

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWarningOverlay);
        }

        // Cek pelanggaran saat level dimulai
        int pelanggaran = PlayerPrefs.GetInt("PelanggaranMakan", 0);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (pelanggaran >= 4 && currentSceneIndex != 0)
        {
            Debug.Log("Pelanggaran mencapai 4 saat masuk level ini, pindah ke EndingScene.");
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        Debug.Log("Pelanggaran Makan di Start: " + pelanggaran);
    }


    public void OnNextLevelButtonPressed()
    {
        // Cek apakah makan belum dipilih, dan warning belum muncul
        if (!makanToggle.isOn && !warningSudahMuncul)
        {
            warningOverlay.SetActive(true);
            warningSudahMuncul = true;
            return;
        }

        // Terapkan pilihan ke BoxPenyimpanan jika ada
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        // Simpan sisa uang
        int sisa = int.Parse(sisaUangText.text.Split(' ')[1].Replace("Rp", "").Replace(",", ""));
        PlayerPrefs.SetInt("SisaUang", sisa);

        // Ambil index scene saat ini sekali saja
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Tambah pelanggaran jika toggle makan tidak dicentang
        if (!makanToggle.isOn)
        {
            int pelanggaran = PlayerPrefs.GetInt("PelanggaranMakan", 0);
            pelanggaran++;
            PlayerPrefs.SetInt("PelanggaranMakan", pelanggaran);
            PlayerPrefs.Save();

            Debug.Log("Pelanggaran Makan setelah update: " + pelanggaran);

            // Jika pelanggaran mencapai 4 dan bukan di level awal (misal index 0), pindah ke Ending
            if (pelanggaran >= 4 && currentSceneIndex != 0)
            {
                Debug.Log("Pelanggaran mencapai 4, pindah ke EndingScene.");
                SceneManager.LoadScene(endingSceneName);
                return;
            }
        }

        // Lanjut ke level berikutnya
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
    }
}
