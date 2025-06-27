using UnityEngine;

public class ChapterProgressSaver : MonoBehaviour
{
    public int chapterJustCompleted = 1;

    public void SaveProgress()
    {
        int currentUnlocked = PlayerPrefs.GetInt("ChapterUnlocked", 1);

        if (chapterJustCompleted + 1 > currentUnlocked)
        {
            PlayerPrefs.SetInt("ChapterUnlocked", chapterJustCompleted + 1);
            PlayerPrefs.Save();
        }
    }
}
