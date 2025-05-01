using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text timerText;

    void Update()
    {
        float t = Mathf.Max(0, gameManager.gameTime);
        timerText.text = "Time: " + t.ToString("0");
    }
}
