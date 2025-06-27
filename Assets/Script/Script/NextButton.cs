using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextButton : MonoBehaviour
{
    public TextMeshProUGUI sisaUangText;
    public BoxPenyimpanan boxPenyimpanan;

    public Toggle makanToggle;
    public Toggle nabungToggle;
    public GameObject warningOverlay;
    public GameObject financeSummaryPanel;
    public Button closeButton;
    public Button btnLanjut;
    public Button debugResetButton; // optional untuk testing di UI

    public string endingSceneName = "EndingScene";
    public string endingSceneName2 = "Ending2";
    public string endingSceneName3 = "Ending3";

    private bool pelanggaranSudahDihitung = false;
    private bool overlaySudahDibuka = false;
    private bool ringkasanSudahDitampilkan = false;

    void Start()
    {
        // Cek nama scene atau buildIndex untuk scene awal
        string sceneName = SceneManager.GetActiveScene().name;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneName == "3DLV1 (Milih Sampah)" || sceneIndex == 2) // ganti sesuai nama/index scene pertamamu
        {
            Debug.Log("🔁 Reset semua data karena ini scene awal");

            PlayerPrefs.DeleteKey("SisaUang");
            PlayerPrefs.DeleteKey("TotalTabungan");
            PlayerPrefs.DeleteKey("PelanggaranMakan");
            PlayerPrefs.DeleteKey("PelanggaranNabung");
            PlayerPrefs.DeleteKey("BuffJajanAktif");
            PlayerPrefs.DeleteKey("TidakMakanKemarin");

            PlayerPrefs.SetInt("SisaUang", 0); // uang awal
            PlayerPrefs.SetInt("TotalTabungan", 0);
            PlayerPrefs.SetInt("PelanggaranMakan", 0);
            PlayerPrefs.SetInt("PelanggaranNabung", 0);
            PlayerPrefs.SetInt("BuffJajanAktif", 0);
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
            PlayerPrefs.Save();
        }

        // Debug log untuk testing
        Debug.Log("📌 Pelanggaran Makan: " + PlayerPrefs.GetInt("PelanggaranMakan"));
        Debug.Log("📌 Pelanggaran Nabung: " + PlayerPrefs.GetInt("PelanggaranNabung"));
        Debug.Log("📌 Sisa Uang: " + PlayerPrefs.GetInt("SisaUang"));

        // Listener tombol
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseWarningOverlay);
        if (btnLanjut != null)
            btnLanjut.onClick.AddListener(OnNextLevelButtonPressed);
    }





    public void OnNextLevelButtonPressed()
    {
        if (overlaySudahDibuka) return;

        if (!pelanggaranSudahDihitung)
        {
            if (!makanToggle || !nabungToggle)
            {
                Debug.LogError("❌ Toggle makan/nabung belum di-assign!");
                return;
            }

            if (!makanToggle.isOn)
            {
                int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0) + 1;
                PlayerPrefs.SetInt("PelanggaranMakan", pelanggaranMakan);
                PlayerPrefs.Save();

                if (warningOverlay) warningOverlay.SetActive(true);
                if (financeSummaryPanel) financeSummaryPanel.SetActive(false);
                overlaySudahDibuka = true;
                pelanggaranSudahDihitung = true;
                ringkasanSudahDitampilkan = false;
                return;
            }

            if (!nabungToggle.isOn)
            {
                int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0) + 1;
                PlayerPrefs.SetInt("PelanggaranNabung", pelanggaranNabung);
                PlayerPrefs.Save();
            }

            TampilkanRingkasan();
            pelanggaranSudahDihitung = true;
            ringkasanSudahDitampilkan = true;
            return; // tunggu klik lanjut lagi
        }

        if (ringkasanSudahDitampilkan && !overlaySudahDibuka)
        {
            LanjutKeSceneBerikutnya();
        }
    }

    void TampilkanRingkasan()
    {
        if (boxPenyimpanan != null)
            boxPenyimpanan.TerapkanPilihan();

        if (sisaUangText != null && !string.IsNullOrWhiteSpace(sisaUangText.text))
        {
            try
            {
                string[] parts = sisaUangText.text.Split(' ');
                if (parts.Length >= 2)
                {
                    string angkaStr = parts[1].Replace("Rp", "").Replace(",", "");
                    int sisa = int.Parse(angkaStr);
                    PlayerPrefs.SetInt("SisaUang", sisa);
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.LogWarning("⚠️ Format teks sisaUangText tidak sesuai!");
                }
            }
            catch
            {
                Debug.LogError("❌ Gagal parsing teks sisa uang.");
            }
        }
        else
        {
            Debug.LogError("❌ sisaUangText kosong atau belum di-assign!");
        }

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(true);
    }

    public void CloseWarningOverlay()
    {
        if (warningOverlay != null)
            warningOverlay.SetActive(false);

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(true);

        overlaySudahDibuka = false;
        ringkasanSudahDitampilkan = true;
    }

    public void LanjutKeSceneBerikutnya()
    {
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0);
        int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0);
        int sisaUang = PlayerPrefs.GetInt("SisaUang", 0);

        Debug.Log($"▶️ Lanjut dengan: PelanggaranMakan={pelanggaranMakan}, PelanggaranNabung={pelanggaranNabung}, SisaUang={sisaUang}");

        if (pelanggaranMakan >= 4)
        {
            Debug.Log("🚫 Masuk Ending 1 (pelanggaran makan)");
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        if (pelanggaranNabung >= 10)
        {
            Debug.Log("🚫 Masuk Ending 2 (pelanggaran nabung)");
            SceneManager.LoadScene(endingSceneName2);
            return;
        }

        if (sisaUang <= 0)
        {
            Debug.Log("🚫 Masuk Ending 3 (uang habis)");
            SceneManager.LoadScene(endingSceneName3);
            return;
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("✅ Pindah ke scene berikutnya.");
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("🎉 Semua level selesai.");
        }
    }

}
