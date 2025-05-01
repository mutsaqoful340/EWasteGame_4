using UnityEngine;
using TMPro;

public class BoxPenyimpanan : MonoBehaviour
{
    public float totalTime = 300f;
    private float timer;

    public int maxItems = 3;  // Maksimum item untuk satu jenis
    private int currentItems = 0;

    public GameObject winPanel;
    public GameObject buttonsPanel;
    public TextMeshProUGUI winText;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI moneyText;

    private bool isGameOver = false;
    private int initialReward = 50000;
    private int currentReward;
    private int lastMinuteChecked = -1;

    private int topEnclosureCount = 0;  // Hitung berapa kali "Top_Enclosure" dimasukkan

    void Start()
    {
        timer = totalTime;
        currentReward = initialReward;
        UpdateTimerUI();
        UpdateMoneyUI();

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateTimerUI();

        // Hitung menit yang sudah berlalu dan kurangi reward jika perlu
        int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
        if (minutesPassed > lastMinuteChecked)
        {
            lastMinuteChecked = minutesPassed;
            currentReward = Mathf.Max(0, initialReward - (minutesPassed * 10000));
            UpdateMoneyUI();
        }

        if (timer <= 0f)
        {
            GameOver(false);
        }
    }

    // Fungsi AddItem yang diubah untuk memeriksa jumlah item
    public void AddItem(string itemType)
    {
        if (isGameOver) return;

        if (itemType == "Top_Enclosure")
        {
            if (topEnclosureCount >= maxItems)
            {
                Debug.Log("Batas item Top_Enclosure sudah tercapai.");
                return;  // Tidak bisa menambahkan item lagi jika sudah mencapai batas
            }
            topEnclosureCount++;  // Tambahkan hitungan untuk Top_Enclosure
        }

        currentItems++;  // Tambahkan total item yang dimasukkan
        Debug.Log($"Item ditambahkan: {currentItems}/{maxItems}");

        if (currentItems >= maxItems)
        {
            GameOver(true);  // Menang jika jumlah item sudah mencapai max
        }
    }

    // Fungsi untuk mengurangi jumlah item jika dihancurkan di TrashZone
    public void RemoveItem()
    {
        currentItems--;
        Debug.Log($"Item dihancurkan. Item sekarang: {currentItems}/{maxItems}");
    }

    public bool IsFull()
    {
        return currentItems >= maxItems;
    }

    void GameOver(bool isWin)
    {
        isGameOver = true;

        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(true);

        if (winText != null)
        {
            winText.gameObject.SetActive(true);

            if (isWin)
            {
                winText.text = "Kamu Menang!\nUpah: Rp" + currentReward.ToString("N0");
            }
            else
            {
                currentReward = 0;
                winText.text = "Kamu Kalah!\nWaktu Habis.";
            }
        }

        UpdateMoneyUI();
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
