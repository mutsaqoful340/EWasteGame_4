using UnityEngine;
using TMPro;

public class BoxPenyimpanan : MonoBehaviour
{
    public float totalTime = 300f;
    private float timer;

    public int maxItems = 2;
    private int currentItems = 0;

    public GameObject winPanel;
    public GameObject buttonsPanel;
    public TextMeshProUGUI winText;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI moneyText;

    private bool isGameOver = false;
    private int money = 0;

    void Start()
    {
        timer = totalTime;
        UpdateTimerUI();
        UpdateMoneyUI();

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0f)
        {
            timer = 0f;
            GameOver(false);
        }
    }

    public void AddItem()
    {
        if (isGameOver || IsFull()) return;

        currentItems++;
        Debug.Log($"Item ditambahkan: {currentItems}/{maxItems}");

        if (currentItems >= maxItems)
        {
            GameOver(true);
        }
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

        if (isWin)
        {
            int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
            int reward = Mathf.Max(0, 50000 - (minutesPassed * 10000));
            money += reward;

            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                winText.text = "Kamu Menang!\nUpah: Rp" + reward.ToString("N0");
            }
        }
        else
        {
            if (winText != null)
            {
                winText.gameObject.SetActive(true);
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
            moneyText.text = "Uang: Rp" + money.ToString("N0");
    }
}
