using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameTime = 60f; // durasi permainan
    public int money = 0; // uang terkumpul
    public int targetMoney = 100; // target uang untuk menang

    public WinUIManager winUIManager; // drag manual dari Inspector
    public GameObject losePanel; // buat panel kalah kalau perlu

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        isGameOver = true;

        if (money >= targetMoney)
        {
            winUIManager.ShowWinUI();
        }
        else
        {
            losePanel.SetActive(true); // panel kalah (kalau dipakai)
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}
