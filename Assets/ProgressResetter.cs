using UnityEngine;

public class ProgressResetter : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("ChapterUnlocked"); // hapus hanya kunci unlock chapter
        PlayerPrefs.Save();

        Debug.Log("🔄 ChapterUnlocked dihapus! Progres kembali ke awal (hanya Chapter 1 yang terbuka).");
    }
}
