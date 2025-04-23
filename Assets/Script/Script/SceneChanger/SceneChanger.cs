using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Ganti ke scene berdasarkan nama
    public void GantiScene(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }

    // Ganti ke scene berdasarkan index
    public void GantiSceneIndex(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }

    // 🔁 Ulangi scene saat ini
    public void UlangiLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
