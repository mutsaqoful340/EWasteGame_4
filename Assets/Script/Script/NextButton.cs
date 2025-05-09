using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextButton : MonoBehaviour
{
    public BoxPenyimpanan boxPenyimpanan;

    public void OnNextLevelButtonPressed()
    {
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        // Ambil index scene sekarang
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Cek apakah next scene masih dalam daftar
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Semua level sudah selesai!");
            // Kamu bisa tambahkan scene khusus seperti "GameSelesai" kalau mau
        }
    }
}
