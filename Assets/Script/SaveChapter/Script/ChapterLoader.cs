using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterLoader : MonoBehaviour
{
    public void LoadChapterScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
