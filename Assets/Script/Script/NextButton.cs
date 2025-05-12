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
    public string endingSceneName = "EndingScene"; // Nama scene ending

    void Start()
    {
        // Reset pelanggaran hanya pada level pertama (buildIndex == 0)
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.DeleteKey("PelanggaranMakan"); // Hapus data pelanggaran pada level pertama
            Debug.Log("Pelanggaran Makan direset di level pertama");
        }

        // Cek pelanggaran pada saat level dimulai
        int pelanggaran = PlayerPrefs.GetInt("PelanggaranMakan", 0);

        // Debug log untuk pelanggaran yang ada
        Debug.Log("Pelanggaran Makan pada Start level: " + pelanggaran);

        // Cek apakah pelanggaran sudah mencapai 4 → langsung ke ending
        if (pelanggaran >= 4)
        {
            Debug.Log("Pelanggaran mencapai 4, langsung pindah ke EndingScene.");
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        // Jika close button ada, tambahkan listener untuk menutup overlay
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWarningOverlay);
        }
    }

    public void OnNextLevelButtonPressed()
    {
        // Jika makan belum dipilih, dan warning belum muncul
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
        PlayerPrefs.Save(); // Simpan perubahan uang ke PlayerPrefs

        // Tambah pelanggaran jika tidak makan
        if (!makanToggle.isOn)
        {
            int pelanggaran = PlayerPrefs.GetInt("PelanggaranMakan", 0);
            pelanggaran++;
            PlayerPrefs.SetInt("PelanggaranMakan", pelanggaran);
            PlayerPrefs.Save();  // Jangan lupa simpan perubahan

            Debug.Log("Pelanggaran Makan setelah update: " + pelanggaran);

            // Jika pelanggaran sudah mencapai 4, langsung ke EndingScene
            if (pelanggaran >= 4)
            {
                Debug.Log("Pelanggaran mencapai 4, pindah ke EndingScene.");
                SceneManager.LoadScene(endingSceneName);
                return;
            }
        }

        // Lanjut ke level berikutnya
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
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
