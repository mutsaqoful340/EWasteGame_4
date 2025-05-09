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

        // Reset pelanggaran saat game mulai untuk debugging
        if (!PlayerPrefs.HasKey("PelanggaranMakan"))
        {
            PlayerPrefs.SetInt("PelanggaranMakan", 0); // Mulai dari 0 jika belum ada
            PlayerPrefs.Save();
        }

        Debug.Log("Pelanggaran Makan di Start: " + PlayerPrefs.GetInt("PelanggaranMakan"));
    }

    public void OnNextLevelButtonPressed()
    {
        // Cek apakah makan belum dipilih, dan warning belum muncul
        if (!makanToggle.isOn && !warningSudahMuncul)
        {
            warningOverlay.SetActive(true);
            warningSudahMuncul = true; // Tandai agar warning hanya muncul sekali
            return;
        }

        // Terapkan pilihan ke BoxPenyimpanan jika ada
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        // Simpan sisa uang yang ada di UI
        int sisa = int.Parse(sisaUangText.text.Split(' ')[1].Replace("Rp", "").Replace(",", ""));
        PlayerPrefs.SetInt("SisaUang", sisa);

        // Periksa apakah toggle Makan belum dipilih, jika ya tambah pelanggaran
        if (!makanToggle.isOn)
        {
            int pelanggaran = PlayerPrefs.GetInt("PelanggaranMakan", 0);  // Ambil nilai pelanggaran yang ada
            pelanggaran++;  // Tambah pelanggaran
            PlayerPrefs.SetInt("PelanggaranMakan", pelanggaran); // Simpan pelanggaran ke PlayerPrefs
            PlayerPrefs.Save(); // Jangan lupa simpan perubahan ke PlayerPrefs

            Debug.Log("Pelanggaran Makan setelah update: " + pelanggaran);  // Cek apakah pelanggaran terupdate

            // Cek apakah pelanggaran >= 4
            if (pelanggaran >= 4)
            {
                Debug.Log("Pelanggaran mencapai 4, pindah ke EndingScene.");
                SceneManager.LoadScene(endingSceneName);  // Pindah ke scene Ending jika pelanggaran >= 4
                return;
            }
        }

        // Lanjutkan ke level berikutnya
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);  // Pindah ke scene berikutnya
        }
        else
        {
            Debug.Log("Semua level sudah selesai!");  // Semua level selesai
        }
    }

    public void CloseWarningOverlay()
    {
        warningOverlay.SetActive(false);  // Tutup overlay jika user menekan tombol Close
    }
}
