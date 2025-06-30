using UnityEngine;

public class AutoResetProgress : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("ChapterUnlocked");
        PlayerPrefs.Save();
        Debug.Log("✅ Progress chapter telah direset!");
    }
}
