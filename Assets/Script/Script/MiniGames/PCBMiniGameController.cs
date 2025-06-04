using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PCBMiniGameController : MonoBehaviour
{
    public TextMeshProUGUI statusText; // Drag komponen Text Status di Inspector
    public GameObject miniGamePanel;   // Drag panel mini game dari Hierarchy

    private List<string> correctSequence = new List<string> { "Blower", "Cairan", "Sikat", "Lap" };
    private List<string> playerSequence = new List<string>();

    void Start()
    {
        if (statusText == null)
            Debug.LogWarning("⚠️ statusText belum di-assign di Inspector.");

        if (miniGamePanel == null)
            Debug.LogWarning("⚠️ miniGamePanel belum di-assign di Inspector.");
        else
            miniGamePanel.SetActive(true); // Pastikan panel aktif saat mini game mulai
    }

    public void OnToolClicked(string toolName)
    {
        playerSequence.Add(toolName);
        CheckSequence();
    }

    void CheckSequence()
    {
        int i = playerSequence.Count - 1;

        if (correctSequence[i] != playerSequence[i])
        {
            statusText.text = "❌ Salah urutan! PCB rusak.";
            Invoke(nameof(ResetPuzzle), 1.5f);
            return;
        }

        if (playerSequence.Count == correctSequence.Count)
        {
            statusText.text = "✅ PCB bersih sempurna!";
            Invoke(nameof(EndMiniGame), 1.5f);
        }
        else
        {
            statusText.text = $"✔️ Langkah {playerSequence.Count}/{correctSequence.Count} benar!";
        }
    }

    void ResetPuzzle()
    {
        playerSequence.Clear();
        statusText.text = "Coba lagi dari awal.";
    }

    void EndMiniGame()
    {
        if (miniGamePanel != null)
            miniGamePanel.SetActive(false); // Nonaktifkan panel saat selesai

        var manager = FindObjectOfType<MiniGameManager>();
        if (manager != null)
            manager.EndMiniGame();
        else
            Debug.LogWarning("MiniGameManager tidak ditemukan!");
    }
}
