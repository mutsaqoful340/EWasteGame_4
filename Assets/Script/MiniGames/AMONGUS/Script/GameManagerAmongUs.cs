using UnityEngine;
using TMPro;

public class GameManagerAmongUs : MonoBehaviour
{
    public int totalCables = 3; // Ganti sesuai jumlah kabel
    private int connectedCount = 0;

    public TextMeshProUGUI feedbackText;

    public void CableConnected()
    {
        connectedCount++;

        if (connectedCount >= totalCables)
        {
            Debug.Log("🎉 Semua kabel terhubung!");
            feedbackText.text = "🎉 Semua kabel berhasil disambung!";
            feedbackText.color = Color.yellow;
        }
    }
}
