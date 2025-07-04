using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BoxPenyimpanan : MonoBehaviour
{
    [Header("Ending VN")]
    public VNDialogManager vnDialogManager;
    public List<VNDialog> vnDialogAkhir;

    [Header("Transisi Setelah VN")]
    public GameObject panelSetelahVN;
    public Button btnLanjutSetelahVN;

    private bool sudahMakanHariIni = false;
    private int hariTidakMakan = 0;

    public float totalTime = 300f;
    private float timer;

    public int maxItems = 3;
    private int currentItems = 0;

    [Header("Pelanggaran")]
    private List<string> daftarPelanggaran = new List<string>();

    public GameObject financeSummaryPanel;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI pemasukanText;
    public TextMeshProUGUI sisaUangText;
    public TextMeshProUGUI tabunganText;

    public Toggle toggleMakan;
    public Toggle toggleNabung;
    public Toggle toggleJajan;

    public Button btnLanjut;
    public Button btnAkhiriGame;

    private bool isGameOver = false;

    [Header("Reward Settings")]
    public int initialReward = 50000;
    private int currentReward;

    private int totalTabungan = 0;
    private int lastMinuteChecked = -1;

    private int makanCost = 15000;
    private int nabungCost = 15000;
    private int jajanCost = 5000;

    private bool buffJajanSudahDipakai = false;
    private bool jajanSudahDiterapkan = false;
    private bool isLevel1 = false;

    void Start()
    {
        timer = totalTime;

        if (PlayerPrefs.GetInt("BuffJajanAktif", 0) == 1)
        {
            timer += 300f;
            PlayerPrefs.SetInt("BuffJajanAktif", 0);
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        isLevel1 = currentSceneName == "DEMO3DLV1";

        if (isLevel1)
        {
            PlayerPrefs.DeleteKey("SisaUang");
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
            currentReward = initialReward;
            totalTabungan = 0;
            PlayerPrefs.SetInt("TotalTabungan", totalTabungan);
        }
        else
        {
            currentReward = PlayerPrefs.GetInt("SisaUang", 0) + initialReward;
            totalTabungan = PlayerPrefs.GetInt("TotalTabungan", 0);
        }

        UpdateMoneyUI();
        UpdateTimerUI();

        if (tabunganText != null)
            tabunganText.text = "Rp" + totalTabungan.ToString("N0");

        financeSummaryPanel?.SetActive(false);
        btnAkhiriGame?.gameObject.SetActive(false);

        if (btnLanjut != null)
        {
            btnLanjut.gameObject.SetActive(false);
            btnLanjut.onClick.AddListener(OnNextButtonClicked);
        }

        if (panelSetelahVN != null)
            panelSetelahVN.SetActive(false);

        UpdateSisaUang();
    }

    void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        timer = Mathf.Max(timer, 0f);

        UpdateTimerUI();

        if (!isLevel1)
        {
            int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
            if (minutesPassed > lastMinuteChecked)
            {
                lastMinuteChecked = minutesPassed;
                currentReward = Mathf.Max(0, currentReward - 10000);
                UpdateMoneyUI();
            }
        }

        if (timer <= 0f)
        {
            GameOver();
        }
    }

    public void AddItem(string itemType)
    {
        if (isGameOver) return;
        currentItems++;

        if (currentItems >= maxItems)
        {
            StartCoroutine(DelayGameOver()); // Gunakan coroutine delay
        }
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(1f); // Atur sesuai durasi animasi penghancuran
        GameOver();
    }

    public void RemoveItem()
    {
        currentItems--;
    }

    public bool IsFull()
    {
        return currentItems >= maxItems;
    }

    public void CatatPelanggaran(string pesan)
    {
        daftarPelanggaran.Add(pesan);
        Debug.Log("Pelanggaran dicatat: " + pesan);
    }

    void GameOver()
    {
        isGameOver = true;

        if (timer <= 0f && isLevel1)
        {
            currentReward = Mathf.Max(currentReward, 10000);
        }
        else if (timer <= 0f)
        {
            currentReward = 0;
        }

        PlayerPrefs.SetInt("SisaUang", currentReward);
        PlayerPrefs.SetInt("TotalTabungan", totalTabungan);
        PlayerPrefs.Save();

        if (vnDialogManager != null && vnDialogAkhir != null && vnDialogAkhir.Count > 0)
        {
            vnDialogManager.isVNEnding = true;
            vnDialogManager.StartVN(vnDialogAkhir);
            StartCoroutine(TungguVNSelesai());
        }
        else
        {
            ShowFinanceSummary();
        }
    }

    IEnumerator TungguVNSelesai()
    {
        yield return new WaitUntil(() => !vnDialogManager.gameObject.activeInHierarchy);

        if (panelSetelahVN != null)
        {
            panelSetelahVN.SetActive(true);

            // Tambahkan listener hanya sekali
            btnLanjutSetelahVN.onClick.RemoveAllListeners();
            btnLanjutSetelahVN.onClick.AddListener(() =>
            {
                panelSetelahVN.SetActive(false);
                ShowFinanceSummary();
            });
        }
        else
        {
            ShowFinanceSummary(); // fallback kalau panel transisi tidak ada
        }
    }


    void ShowFinanceSummary()
    {
        financeSummaryPanel?.SetActive(true);

        if (pemasukanText != null)
            pemasukanText.text = "Rp" + currentReward.ToString("N0");

        UpdateSisaUang();

        toggleMakan?.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleNabung?.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleJajan?.onValueChanged.AddListener(delegate { UpdateSisaUang(); });

        btnLanjut?.gameObject.SetActive(true);
        btnAkhiriGame?.gameObject.SetActive(true);
    }

    void UpdateSisaUang()
    {
        int totalPengeluaran = 0;

        if (toggleMakan?.isOn == true) totalPengeluaran += makanCost;
        if (toggleNabung?.isOn == true) totalPengeluaran += nabungCost;
        if (toggleJajan?.isOn == true) totalPengeluaran += jajanCost;

        int sisa = Mathf.Max(0, currentReward - totalPengeluaran);
        sisaUangText.text = "Sisa: Rp" + sisa.ToString("N0");

        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();
    }

    public void TerapkanPilihan()
    {
        if (isLevel1 || jajanSudahDiterapkan) return;

        bool playerMakan = toggleMakan?.isOn == true;
        bool playerJajan = toggleJajan?.isOn == true;
        bool playerNabung = toggleNabung?.isOn == true;

        if (playerJajan && !buffJajanSudahDipakai && currentReward >= jajanCost)
        {
            currentReward -= jajanCost;
            timer += 300f;
            buffJajanSudahDipakai = true;
            PlayerPrefs.SetInt("BuffJajanAktif", 1);
        }

        if (playerMakan && currentReward >= makanCost)
        {
            currentReward -= makanCost;
            sudahMakanHariIni = true;
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
        }
        else
        {
            sudahMakanHariIni = false;
            PlayerPrefs.SetInt("TidakMakanKemarin", 1);
        }

        if (playerNabung && currentReward >= nabungCost)
        {
            currentReward -= nabungCost;
            totalTabungan += nabungCost;
            PlayerPrefs.SetInt("TotalTabungan", totalTabungan);

            if (tabunganText != null)
                tabunganText.text = "Tabungan Rp" + totalTabungan.ToString("N0");
        }

        PlayerPrefs.SetInt("SisaUang", currentReward);
        PlayerPrefs.Save();

        jajanSudahDiterapkan = true;

        UpdateMoneyUI();
        UpdateTimerUI();
    }

    public void OnNextButtonClicked()
    {
        TerapkanPilihan();
        GoToNextLevel();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateMoneyUI()
    {
        moneyText.text = "Rp" + currentReward.ToString("N0");
    }

    void GoToNextLevel()
    {
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

    public void TampilkanRingkasanLangsungDariVN()
    {
        isGameOver = true;
        ShowFinanceSummary();
    }

    public List<string> GetPelanggaranList()
    {
        return daftarPelanggaran;
    }
}
