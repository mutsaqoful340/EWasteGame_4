using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxPenyimpanan : MonoBehaviour
{
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

    private bool isGameOver = false;
    private int initialReward = 50000;
    private int currentReward;
    private int lastMinuteChecked = -1;

    private int makanCost = 15000;
    private int nabungCost = 15000;
    private int jajanCost = 10000;

    private float delayToSummary = 2f;

    void Start()
    {
        timer = totalTime;
        currentReward = initialReward;
        UpdateTimerUI();
        UpdateMoneyUI();

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        // Update timer
        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateTimerUI();

        // Menghitung reward berdasarkan waktu yang tersisa
        int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
        if (minutesPassed > lastMinuteChecked)
        {
            lastMinuteChecked = minutesPassed;
            currentReward = Mathf.Max(0, initialReward - (minutesPassed * 10000));
            UpdateMoneyUI();
        }

        // Jika waktu habis, panggil GameOver()
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

        // Pastikan currentReward diset ke 0 jika waktu habis
        if (timer <= 0f)
        {
            currentReward = 0;
        }

        ShowFinanceSummary();  // Menampilkan ringkasan keuangan setelah game selesai
    }

    void ShowFinanceSummary()
    {
        // Menampilkan panel ringkasan keuangan setelah game selesai
        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(true);

        // Menampilkan pemasukan sesuai currentReward
        if (pemasukanText != null)
            pemasukanText.text = "Pemasukan: Rp" + currentReward.ToString("N0");

        // Perbarui sisa uang
        UpdateSisaUang();

        // Tambahkan listener agar toggle update sisa uang saat dicentang/diubah
        toggleMakan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleNabung.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleJajan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
    }

    void UpdateSisaUang()
    {
        int totalPengeluaran = 0;

        // Periksa toggle dan hitung pengeluaran
        if (toggleMakan.isOn) totalPengeluaran += makanCost;
        if (toggleNabung.isOn) totalPengeluaran += nabungCost;
        if (toggleJajan.isOn) totalPengeluaran += jajanCost;

        // Hitung sisa uang
        int sisa = currentReward - totalPengeluaran;

        // Pastikan tidak ada nilai yang negatif
        sisa = Mathf.Max(0, sisa);

        // Perbarui UI dengan nilai sisa uang
        if (sisaUangText != null)
            sisaUangText.text = "Sisa: Rp" + sisa.ToString("N0");
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
}
