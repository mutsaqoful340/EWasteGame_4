using UnityEngine;
using UnityEngine.UI;

public class ChapterUnlockManager : MonoBehaviour
{
    public Button[] chapterButtons; // Drag Chap_1, Chap_2, dst di Inspector

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("ChapterUnlocked", 1); // default hanya Chap_1 terbuka

        for (int i = 0; i < chapterButtons.Length; i++)
        {
            bool isUnlocked = i < unlocked;
            chapterButtons[i].interactable = isUnlocked;

            // Optional: tampilin/hidden ikon gembok
            Transform lockIcon = chapterButtons[i].transform.Find("LockIcon");
            if (lockIcon != null)
                lockIcon.gameObject.SetActive(!isUnlocked);
        }
    }



}
