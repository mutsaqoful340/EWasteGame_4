using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject miniGameCamera;
    public GameObject pcbOnTable;

    public GameObject miniGameUIPanel;   // Panel UI mini game
    public GameObject gameplayUI;         // UI gameplay utama (timer, tabungan, dll)

    public void StartMiniGame()
    {
        mainCamera.SetActive(false);
        miniGameCamera.SetActive(true);
        pcbOnTable.SetActive(true);

        miniGameUIPanel.SetActive(true);
        gameplayUI.SetActive(false);
    }

    public void EndMiniGame()
    {
        miniGameCamera.SetActive(false);
        mainCamera.SetActive(true);
        pcbOnTable.SetActive(false);

        miniGameUIPanel.SetActive(false);
        gameplayUI.SetActive(true);
    }
}

