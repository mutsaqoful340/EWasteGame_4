using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BoxPenyimpanan : MonoBehaviour
{
    [Header("Ending")]
    public GameObject endingPanel;

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

    public Toggle toggleMakan;
    public Toggle toggleNabung;
    public Toggle toggleJajan;

    public Button btnLihatRingkasan;
    public Button btnAkhiriGame;

    private bool isGameOver = false;

    [Header("Reward Settings")]
    public int initialReward = 50000;
    private int currentReward;

    private int lastMinuteChecked = -1;

    private int makanCost = 15000;
    private int nabungCost = 15000;
    private int jajanCost = 10000;

    private float delayToSummary = 2f;

    private bool buffJajanSudahDipakai = false;
    private bool jajanSudahDiterapkan = false;

    void Start()
    {
        timer = totalTime;

        if (PlayerPrefs.GetInt("BuffJajanAktif", 0) == 1)
        {
            timer += 300f;
            PlayerPrefs.SetInt("BuffJajanAktif", 0);
        }

        if (SceneManager.GetActiveScene().name == "3DLV1")
        {
            PlayerPrefs.DeleteKey("SisaUang");
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
        }

        int sisaUangDariLevelSebelumnya = PlayerPrefs.GetInt("SisaUang", 0);
        currentReward = sisaUangDariLevelSebelumnya + initialReward;

        btnAkhiriGame.gameObject.SetActive(false);


        UpdateTimerUI();
        UpdateMoneyUI();
    }

    void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateTimerUI();

        int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
        if (minutesPassed > lastMinuteChecked)
        {
            lastMinuteChecked = minutesPassed;
            currentReward = Mathf.Max(0, currentReward - (minutesPassed * 10000));
            UpdateMoneyUI();
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

        if (timer <= 0f)
        {
            currentReward = 0;
        }

        ShowFinanceSummary();

        // Setelah delay tertentu, pindah ke level berikutnya
         // delayToSummary adalah waktu delay yang diinginkan
    }

    void ShowFinanceSummary()
    {
        if (financeSummaryPanel != null && isGameOver)
        {
            financeSummaryPanel.SetActive(true);
        }

        if (pemasukanText != null)
            pemasukanText.text = "Pemasukan: Rp" + currentReward.ToString("N0");

        UpdateSisaUang();

        toggleMakan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleNabung.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleJajan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });

        if (btnAkhiriGame != null)
        {
            btnAkhiriGame.gameObject.SetActive(true);
        }
    }

    void UpdateSisaUang()
    {
        int totalPengeluaran = 0;

        if (toggleMakan.isOn) totalPengeluaran += makanCost;
        if (toggleNabung.isOn) totalPengeluaran += nabungCost;
        if (toggleJajan.isOn) totalPengeluaran += jajanCost;

        int sisa = currentReward - totalPengeluaran;
        sisa = Mathf.Max(0, sisa);

        if (sisaUangText != null)
            sisaUangText.text = "Sisa: Rp" + sisa.ToString("N0");

        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();
    }

    public void TerapkanPilihan()
    {
        if (jajanSudahDiterapkan) return;

        bool playerMakan = toggleMakan.isOn;
        bool playerJajan = toggleJajan.isOn;

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
        }
        else
        {
            sudahMakanHariIni = false;
        }

        if (sudahMakanHariIni)
        {
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
        }
        else
        {
            PlayerPrefs.SetInt("TidakMakanKemarin", 1);
        }

        PlayerPrefs.SetInt("SisaUang", currentReward);
        PlayerPrefs.Save();


        UpdateMoneyUI();
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Uang: Rp" + currentReward.ToString("N0");
    }

    void GoToNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }



}
