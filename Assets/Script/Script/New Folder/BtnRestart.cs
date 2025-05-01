using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnRestart : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
