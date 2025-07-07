using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerAmongUs : MonoBehaviour
{
    public int totalCables = 4;
    private int connectedCount = 0;

    public TextMeshProUGUI feedbackText;
    public GameObject winPanel;
    public Button btnLanjut;

    public string nextSceneName = "SceneLanjut"; // 👉 Ganti dengan nama scene berikutnya

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (btnLanjut != null)
            btnLanjut.onClick.AddListener(LanjutKeSceneBerikutnya);
    }

    public void CableConnected()
    {
        connectedCount++;

        if (connectedCount >= totalCables)
        {
            Debug.Log("🎉 Semua kabel terhubung!");

            if (feedbackText != null)
            {
                feedbackText.text = "🎉 Semua kabel berhasil disambung!";
                feedbackText.color = Color.yellow;
            }

            if (winPanel != null)
                winPanel.SetActive(true);
        }
    }

    public void LanjutKeSceneBerikutnya()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("❌ Nama scene kosong! Isi di Inspector.");
        }
    }
}
