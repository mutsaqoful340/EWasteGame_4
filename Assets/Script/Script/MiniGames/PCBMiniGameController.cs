using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PCBMiniGameController : MonoBehaviour
{
    public TextMeshProUGUI statusText; // drag komponen Text Status di Inspector

    private List<string> correctSequence = new List<string> { "Blower", "Cairan", "Sikat", "Lap" };
    private List<string> playerSequence = new List<string>();

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
            Invoke("ResetPuzzle", 1.5f);
            return;
        }

        if (playerSequence.Count == correctSequence.Count)
        {
            statusText.text = "✅ PCB bersih sempurna!";

            // Delay sebentar baru keluar mini game supaya pemain bisa lihat status
            Invoke(nameof(EndMiniGame), 1.5f);
        }
        else
        {
            statusText.text = $"✔️ Langkah {playerSequence.Count}/{correctSequence.Count} benar!";
        }
    }

    void EndMiniGame()
    {
        // Panggil MiniGameManager untuk selesaiin mini game dan balik ke gameplay utama
        FindObjectOfType<MiniGameManager>().EndMiniGame();
    }


    void ResetPuzzle()
    {
        playerSequence.Clear();
        statusText.text = "Coba lagi dari awal.";
    }
}
