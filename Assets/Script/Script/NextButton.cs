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

    private bool warningSudahMuncul = false;
    public string endingSceneName = "EndingScene";
    public string endingSceneName2 = "Ending2";

    void Start()
    {
        // Reset pelanggaran hanya pada level pertama
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ResetPelanggaran();
            Debug.Log("Pelanggaran direset di level pertama.");
        }

        int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0);
        int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0);

        Debug.Log("Pelanggaran Makan pada Start: " + pelanggaranMakan);
        Debug.Log("Pelanggaran Nabung pada Start: " + pelanggaranNabung);

        // Langsung ke ending jika pelanggaran terpenuhi
        if (pelanggaranMakan >= 4)
        {
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        if (pelanggaranNabung >= 10)
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
        if (!makanToggle.isOn && !warningSudahMuncul)
        {
            warningOverlay.SetActive(true);
            warningSudahMuncul = true;
            return;
        }

        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        int sisa = int.Parse(sisaUangText.text.Split(' ')[1].Replace("Rp", "").Replace(",", ""));
        PlayerPrefs.SetInt("SisaUang", sisa);

        if (!makanToggle.isOn)
        {
            int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0) + 1;
            PlayerPrefs.SetInt("PelanggaranMakan", pelanggaranMakan);
            Debug.Log("Pelanggaran Makan: " + pelanggaranMakan);
        }

        if (!nabungToggle.isOn)
        {
            int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0) + 1;
            PlayerPrefs.SetInt("PelanggaranNabung", pelanggaranNabung);
            Debug.Log("Pelanggaran Nabung: " + pelanggaranNabung);
        }

        PlayerPrefs.Save();

        if (financeSummaryPanel != null)
        {
            financeSummaryPanel.SetActive(true);
        }
    }

    public void LanjutKeSceneBerikutnya()
    {
        int pelanggaranMakan = PlayerPrefs.GetInt("PelanggaranMakan", 0);
        int pelanggaranNabung = PlayerPrefs.GetInt("PelanggaranNabung", 0);

        if (pelanggaranMakan >= 4)
        {
            SceneManager.LoadScene(endingSceneName);
            return;
        }

        if (pelanggaranNabung >= 10)
        {
            SceneManager.LoadScene(endingSceneName2);
            return;
        }

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Reset pelanggaran jika mau reset setiap pindah level:
            // ResetPelanggaran();

            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Semua level selesai.");
        }
    }

    public void CloseWarningOverlay()
    {
        warningOverlay.SetActive(false);
    }
}
