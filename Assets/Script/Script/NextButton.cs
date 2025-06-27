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

    public string endingSceneName = "EndingScene";
    public string endingSceneName2 = "Ending2";
    public string endingSceneName3 = "Ending3";

    private bool pelanggaranSudahDihitung = false;
    private bool overlaySudahDibuka = false;
    private bool ringkasanSudahDitampilkan = false;

    private bool isLevel1 = false;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Cek apakah ini adalah scene level 1
        if (sceneName.Equals("3DLV1 (Milih Sampah)"))
        {
            isLevel1 = true;
            Debug.Log("🔁 Reset semua data karena ini scene awal");

            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("SisaUang", 0);
            PlayerPrefs.SetInt("TotalTabungan", 0);
            PlayerPrefs.SetInt("PelanggaranMakan", 0);
            PlayerPrefs.SetInt("PelanggaranNabung", 0);
            PlayerPrefs.SetInt("BuffJajanAktif", 0);
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
            PlayerPrefs.Save();
        }

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
            // Kalau bukan level 1, jalankan cek toggle
            if (!isLevel1)
            {
                if (makanToggle == null || nabungToggle == null)
                {
                    Debug.LogError("❌ Toggle makan/nabung belum di-assign di level ini!");
                    return;
                }

                if (!makanToggle.isOn)
                {
                    int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0) + 1;
                    PlayerPrefs.SetInt("PelanggaranMakan", pelanggaranMakan);
                    PlayerPrefs.Save();

                    if (warningOverlay != null) warningOverlay.SetActive(true);
                    if (financeSummaryPanel != null) financeSummaryPanel.SetActive(false);
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
            }

            TampilkanRingkasan();
            pelanggaranSudahDihitung = true;
            ringkasanSudahDitampilkan = true;
            return;
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
                // Ambil angka dari teks (hilangkan "Sisa:", "Rp", spasi, titik)
                string cleanedText = sisaUangText.text
                    .ToLower()                         // biar aman: "SISA", "sisa", "Sisa" semua jadi "sisa"
                    .Replace("sisa:", "")
                    .Replace("sisa", "")
                    .Replace("rp", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Trim();


                if (int.TryParse(cleanedText, out int sisa))
                {
                    PlayerPrefs.SetInt("SisaUang", sisa);
                    PlayerPrefs.Save();
                    Debug.Log("✅ Sisa uang berhasil disimpan: " + sisa);
                }
                else
                {
                    Debug.LogWarning("⚠️ Format teks sisaUangText tidak valid: " + cleanedText);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("❌ Gagal parsing teks sisa uang. Error: " + ex.Message);
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

        // ⛔ Skip ending logic kalau ini adalah level 1
        if (!isLevel1)
        {
            if (pelanggaranMakan >= 4)
            {
                SceneManager.LoadScene(endingSceneName); return;
            }

            if (pelanggaranNabung >= 10)
            {
                SceneManager.LoadScene(endingSceneName2); return;
            }

            if (sisaUang <= 0)
            {
                SceneManager.LoadScene(endingSceneName3); return;
            }
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("🎉 Semua level selesai.");
        }
    }

}
