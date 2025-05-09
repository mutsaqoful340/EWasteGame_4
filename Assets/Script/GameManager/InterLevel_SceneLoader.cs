using UnityEngine;
using UnityEngine.SceneManagement;

public class InterLevel_SceneLoader : MonoBehaviour
{
    [Header("Type your scene name here")]
    public string sceneName;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty! Please type the scene name in the Inspector.");
        }
    }
}
