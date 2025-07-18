using UnityEngine;
using System.Collections;

public class MiniGameManager : MonoBehaviour
{
    public GameObject hpMiniGame;
    public GameObject hpMeja;
    public void StartMiniGame()
    {
        hpMiniGame.SetActive(true);
        hpMeja.SetActive(true);
    }

    public void EndMiniGame()
    {
        hpMiniGame.SetActive(false);
        hpMeja.SetActive(false);
    }
}
