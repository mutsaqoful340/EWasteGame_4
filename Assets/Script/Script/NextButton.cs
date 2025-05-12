using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextButton : MonoBehaviour
{
    public TextMeshProUGUI sisaUangText;
    public BoxPenyimpanan boxPenyimpanan;

    public Toggle makanToggle;
    public Toggle nabungToggle;  // Menambahkan toggle untuk nabung
    public GameObject warningOverlay;
    public Button closeButton;

    private bool warningSudahMuncul = false;
    public string endingSceneName = "EndingScene"; // Nama scene ending
    public string endingSceneName2 = "Ending2";   // Nama scene ending 2 jika tidak nabung

    void Start()
    {
        // Reset pelanggaran hanya pada level pertama (buildIndex == 0)
        // Menghapus reset pelanggaran agar tetap bertahan antar level
        // Jika kamu tidak ingin reset pelanggaran, maka hapus atau komentar bagian berikut:
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // PlayerPrefs.DeleteKey("PelanggaranMakan"); // Hapus data pelanggaran makan
            // PlayerPrefs.DeleteKey("PelanggaranNabung"); // Hapus data pelanggaran nabung
            Debug.Log("Pelanggaran Makan dan Nabung tidak di-reset di level pertama");
        }

        // Cek pelanggaran pada saat level dimulai
        int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0);
        int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0);

        // Debug log untuk pelanggaran yang ada
        Debug.Log("Pelanggaran Makan pada Start level: " + pelanggaranMakan);
        Debug.Log("Pelanggaran Nabung pada Start level: " + pelanggaranNabung);

        // Cek apakah pelanggaran sudah mencapai 4 → langsung ke ending
        if (pelanggaranMakan >= 4)
        {
            Debug.Log("Pelanggaran mencapai 4, langsung pindah ke EndingScene.");
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        // Cek apakah pelanggaran nabung sudah mencapai 10 → langsung ke Ending2
        if (pelanggaranNabung >= 10)
        {
            Debug.Log("Tidak nabung 10 kali, langsung pindah ke Ending2.");
            SceneManager.LoadScene(endingSceneName2);
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

        // Tambah pelanggaran jika tidak nabung
        if (!nabungToggle.isOn)
        {
            int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0);
            pelanggaranNabung++;
            PlayerPrefs.SetInt("PelanggaranNabung", pelanggaranNabung);
            PlayerPrefs.Save();  // Jangan lupa simpan perubahan

            Debug.Log("Pelanggaran Nabung setelah update: " + pelanggaranNabung);

            // Jika pelanggaran nabung sudah mencapai 10, langsung ke Ending2
            if (pelanggaranNabung >= 8)
            {
                Debug.Log("Tidak nabung 10 kali, pindah ke Ending2.");
                SceneManager.LoadScene(endingSceneName2);
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
