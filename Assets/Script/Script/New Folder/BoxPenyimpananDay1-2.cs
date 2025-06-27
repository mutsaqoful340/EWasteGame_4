using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoxPenyimpananSimple : MonoBehaviour
{
    [Header("UI & Panel")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI upahText; // Hanya tampil saat game over
    public GameObject summaryPanel;

    [Header("Game Settings")]
    public float totalTime = 300f;
    private float timer;

    public int maxItems = 3;
    private int currentItems = 0;

    private bool isGameOver = false;

    [Header("Reward Settings")]
    public int initialReward = 50000;
    private int currentReward;

    private int lastMinuteChecked = -1;

    void Start()
    {
        timer = totalTime;
        currentReward = initialReward;

        // Sembunyikan panel ringkasan & teks upah di awal
        if (summaryPanel != null)
            summaryPanel.SetActive(false);

        if (upahText != null)
            upahText.gameObject.SetActive(false);

        UpdateTimerUI();
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
            currentReward = Mathf.Max(0, currentReward - 10000); // hanya -10rb per menit
        }


        if (timer <= 0f)
        {
            GameOver();
        }
    }

    public void AddItem()
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
        currentItems = Mathf.Max(0, currentItems - 1);
        Debug.Log($"Item dihapus. Sekarang: {currentItems}/{maxItems}");
    }

    public bool IsFull()
    {
        return currentItems >= maxItems;
    }


    void GameOver()
    {
        isGameOver = true;

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "3DLV1 (Milih Sampah)" && currentReward <= 0)
        {
            currentReward = 10000; // kasih minimum reward
        }

        PlayerPrefs.SetInt("SisaUang", currentReward);
        PlayerPrefs.Save();

        ShowSummary();
    }




    void ShowSummary()
    {
        Debug.Log("ShowSummary called!");

        if (summaryPanel != null)
        {
            summaryPanel.SetActive(true);
            Debug.Log("Summary panel shown.");
        }

        if (upahText != null)
        {
            upahText.gameObject.SetActive(true);
            upahText.text = "Upah: Rp" + currentReward.ToString("N0");
            Debug.Log("Upah shown: " + currentReward);
        }
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
}
