using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterButtonLoader : MonoBehaviour
{
    public string sceneToLoad; // isi di Inspector

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("📥 Load scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        });
    }
}
