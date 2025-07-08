using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerAmongUs : MonoBehaviour
{
    public int totalCables = 4;
    private int connectedCount = 0;

    [Header("Gameplay UI")]
    public TextMeshProUGUI feedbackText;
    public GameObject winPanel;
    public Button btnLanjut;
    public Button btnUlang; // ✅ Tombol coba lagi

    [Header("Start Panel")]
    public GameObject startPanel;
    public Button btnMulai;

    [Header("Scene")]
    public string nextSceneName = "SceneLanjut";

    void Start()
    {
        if (startPanel != null)
            startPanel.SetActive(true);

        if (btnMulai != null)
            btnMulai.onClick.AddListener(() =>
            {
                if (startPanel != null)
                    startPanel.SetActive(false);
            });

        if (winPanel != null)
            winPanel.SetActive(false);

        if (btnLanjut != null)
            btnLanjut.onClick.AddListener(LanjutKeSceneBerikutnya);

        if (btnUlang != null)
            btnUlang.onClick.AddListener(RestartMinigame); // ✅ Tambahkan listener tombol ulang
    }

    public void CableConnected()
    {
        connectedCount++;

        if (connectedCount >= totalCables)
        {
            Debug.Log("🎉 Semua kabel terhubung!");

            if (feedbackText != null)
            {
                feedbackText.text = "";
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

    public void RestartMinigame()
    {
        Debug.Log("🔁 Ulangi minigame!");

        connectedCount = 0;

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        foreach (StretchCable2D kabel in FindObjectsOfType<StretchCable2D>())
        {
            kabel.ResetConnection();
        }
    }
}
