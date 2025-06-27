using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BoxPenyimpanan : MonoBehaviour
{
    [Header("Ending")]
    public GameObject endingPanel;

    [Header("Panel Notifikasi Bertahap")]
    public GameObject[] notifikasiPanels;

    private bool sudahMakanHariIni = false;
    private int hariTidakMakan = 0;

    public float totalTime = 300f;
    private float timer;

    public int maxItems = 3;
    private int currentItems = 0;

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
    private int jajanCost = 10000;

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
        isLevel1 = currentSceneName == "3DLV1 (Milih Sampah)";

        if (isLevel1)
        {
            PlayerPrefs.DeleteKey("SisaUang");
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
            currentReward = initialReward;
            totalTabungan = 0;
            PlayerPrefs.SetInt("TotalTabungan", totalTabungan);
            PlayerPrefs.Save();
        }
        else
        {
            currentReward = PlayerPrefs.GetInt("SisaUang", 0) + initialReward;
            totalTabungan = PlayerPrefs.GetInt("TotalTabungan", 0);
        }

        UpdateMoneyUI();
        UpdateTimerUI();

        if (tabunganText != null)
            tabunganText.text = "Tabungan Rp" + totalTabungan.ToString("N0");

        if (btnAkhiriGame != null)
            btnAkhiriGame.gameObject.SetActive(false);

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(false);

        if (btnLanjut != null)
        {
            btnLanjut.gameObject.SetActive(false);
            btnLanjut.onClick.AddListener(OnNextButtonClicked);
        }

        UpdateSisaUang();

        Debug.Log($"[Start] currentReward = {currentReward}, totalTabungan = {totalTabungan}");
    }

    void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateTimerUI();

        if (!isLevel1)
        {
            int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
            if (minutesPassed > lastMinuteChecked)
            {
                lastMinuteChecked = minutesPassed;
                currentReward = Mathf.Max(0, currentReward - (minutesPassed * 10000));
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
        Debug.Log($"Item ditambahkan: {currentItems}/{maxItems}");

        if (currentItems >= maxItems)
        {
            GameOver();
        }
    }

    public void RemoveItem()
    {
        currentItems--;
        Debug.Log($"Item dihancurkan. Item sekarang: {currentItems}/{maxItems}");
    }

    public bool IsFull()
    {
        return currentItems >= maxItems;
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

        StartCoroutine(TampilkanNotifikasiPanelsBergantian());
    }


    void ShowFinanceSummary()
    {
        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(true);

        if (pemasukanText != null)
            pemasukanText.text = " Rp" + currentReward.ToString("N0");

        UpdateSisaUang();

        if (toggleMakan != null)
            toggleMakan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });

        if (toggleNabung != null)
            toggleNabung.onValueChanged.AddListener(delegate { UpdateSisaUang(); });

        if (toggleJajan != null)
            toggleJajan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });

        if (btnLanjut != null)
            btnLanjut.gameObject.SetActive(true);
        if (btnAkhiriGame != null)
            btnAkhiriGame.gameObject.SetActive(true);
    }

    void UpdateSisaUang()
    {
        int totalPengeluaran = 0;

        if (toggleMakan != null && toggleMakan.isOn) totalPengeluaran += makanCost;
        if (toggleNabung != null && toggleNabung.isOn) totalPengeluaran += nabungCost;
        if (toggleJajan != null && toggleJajan.isOn) totalPengeluaran += jajanCost;

        int sisa = currentReward - totalPengeluaran;
        sisa = Mathf.Max(0, sisa);

        if (sisaUangText != null)
            sisaUangText.text = "Sisa: Rp" + sisa.ToString("N0");

        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();

        Debug.Log($"[UpdateSisaUang] SisaUang: {sisa}, Tabungan: {totalTabungan}");
    }

    public void TerapkanPilihan()
    {
        if (isLevel1)
        {
            Debug.Log("[TerapkanPilihan] Diabaikan karena masih di Level 1");
            return;
        }

        if (jajanSudahDiterapkan) return;

        // Tambahkan perlindungan null
        if (toggleMakan == null || toggleNabung == null || toggleJajan == null)
        {
            Debug.LogWarning("[TerapkanPilihan] Salah satu toggle null. Pastikan toggle lengkap di scene.");
            return;
        }

        bool playerMakan = toggleMakan.isOn;
        bool playerJajan = toggleJajan.isOn;
        bool playerNabung = toggleNabung.isOn;

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

            Debug.Log($"[TerapkanPilihan] Nabung +{nabungCost}, TotalTabungan: {totalTabungan}");
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
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private IEnumerator TampilkanNotifikasiPanelsBergantian()
    {
        foreach (GameObject panel in notifikasiPanels)
        {
            if (panel != null) panel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Tahan tiap panel selama 1.5 detik
            if (panel != null) panel.SetActive(false);
        }

        ShowFinanceSummary(); // Tampilkan panel keuangan setelah semua notifikasi
    }


    void UpdateMoneyUI()
    {
        if (moneyText != null)
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
            Debug.Log("Semua level selesai.");
        }
    }
}
