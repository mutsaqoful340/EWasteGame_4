using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIManager : MonoBehaviour
{
    public GameObject winPanel;

    public void ShowWinUI()
    {
        winPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Ganti nama scene sesuai project
    }
}
