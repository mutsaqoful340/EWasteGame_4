using UnityEngine;
using UnityEngine.UI; // untuk Toggle, Button, GameObject (GameObject sebenarnya di UnityEngine)
using UnityEngine.SceneManagement;
using TMPro; // untuk TextMeshProUGUI

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

    private bool pelanggaranSudahDihitung = false;
    private bool overlaySudahDibuka = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ResetPelanggaran();
        }

        if (PlayerPrefs.GetInt("PelanggaranMakan", 0) >= 4)
        {
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        if (PlayerPrefs.GetInt("PelanggaranNabung", 0) >= 10)
        {
            SceneManager.LoadScene(endingSceneName2);
            return;
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWarningOverlay);
        }
    }

    void ResetPelanggaran()
    {
        PlayerPrefs.SetInt("PelanggaranMakan", 0);
        PlayerPrefs.SetInt("PelanggaranNabung", 0);
        PlayerPrefs.Save();
    }

    public void OnNextLevelButtonPressed()
    {
        if (!pelanggaranSudahDihitung)
        {
            // Cek makan toggle, kalau belum dicentang tampilkan overlay warning
            if (!makanToggle.isOn)
            {
                int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0) + 1;
                PlayerPrefs.SetInt("PelanggaranMakan", pelanggaranMakan);
                PlayerPrefs.Save();

                warningOverlay.SetActive(true);
                financeSummaryPanel.SetActive(false); // sembunyikan ringkasan
                overlaySudahDibuka = true;
                pelanggaranSudahDihitung = true;

                return; // stop di sini, tunggu user tutup overlay
            }

            // Kalau makan sudah dicentang, hitung pelanggaran nabung saja
            if (!nabungToggle.isOn)
            {
                int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0) + 1;
                PlayerPrefs.SetInt("PelanggaranNabung", pelanggaranNabung);
                PlayerPrefs.Save();
            }

            // Terapkan pilihan dan simpan uang
            TampilkanRingkasan();
            pelanggaranSudahDihitung = true;
        }
        else
        {
            // Kalau pelanggaran sudah dihitung dan overlay sudah dibuka (atau tidak perlu overlay),
            // langsung lanjut ke scene berikutnya
            LanjutKeSceneBerikutnya();
        }
    }

    void TampilkanRingkasan()
    {
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        int sisa = int.Parse(sisaUangText.text.Split(' ')[1].Replace("Rp", "").Replace(",", ""));
        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();

        if (financeSummaryPanel != null)
        {
            financeSummaryPanel.SetActive(true);
        }
    }

    public void CloseWarningOverlay()
    {
        warningOverlay.SetActive(false);
        financeSummaryPanel.SetActive(true);
        overlaySudahDibuka = false;
    }

    public void LanjutKeSceneBerikutnya()
    {
        if (PlayerPrefs.GetInt("PelanggaranMakan", 0) >= 4)
        {
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        if (PlayerPrefs.GetInt("PelanggaranNabung", 0) >= 10)
        {
            SceneManager.LoadScene(endingSceneName2);
            return;
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Semua level selesai.");
        }
    }
}
