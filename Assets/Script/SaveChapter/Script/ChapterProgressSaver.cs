using UnityEngine;

public class ChapterProgressSaver : MonoBehaviour
{
    public int chapterJustCompleted = 1;

    public void SaveProgress()
    {
        int currentUnlocked = PlayerPrefs.GetInt("ChapterUnlocked", 1);

        Debug.Log("🧠 ChapterUnlocked saat ini: " + currentUnlocked);
        Debug.Log("🏁 Baru saja menyelesaikan: Chapter " + chapterJustCompleted);

        if (chapterJustCompleted + 1 > currentUnlocked)
        {
            PlayerPrefs.SetInt("ChapterUnlocked", chapterJustCompleted + 1);
            PlayerPrefs.Save(); // ⬅️ Ini WAJIB
            Debug.Log("✅ Progress DISIMPAN! Sekarang terbuka sampai Chapter " + (chapterJustCompleted + 1));
        }
        else
        {
            Debug.Log("📎 Tidak menyimpan karena sudah terbuka sebelumnya.");
        }
    }
}
