using UnityEngine;
using System.Collections.Generic;

public class TutorialTrigger : MonoBehaviour
{
    // Menyimpan jumlah klik per objectID
    private static Dictionary<string, int> objectClickCounts = new Dictionary<string, int>();

    [Header("Tutorial Settings")]
    public string objectID;                        // Unik per objek, misalnya "BotolA", "LaptopB"
    public GameObject panelObject;                 // Panel tutorial
    public Animator tutorialAnimator;              // Animator panel
    public string animationName = "SlideIn";       // Nama animasi
    public float displayDuration = 3f;             // Lama tampil panel

    [Header("Optional Behavior")]
    public bool requireSecondClick = false;        // Centang jika butuh klik 2x

    private bool tutorialActive = false;

    void OnMouseDown()
    {
        // Cek apakah ID valid
        if (string.IsNullOrEmpty(objectID))
        {
            Debug.LogWarning("objectID belum diisi di: " + gameObject.name);
            return;
        }

        // Inisialisasi klik count jika belum ada
        if (!objectClickCounts.ContainsKey(objectID))
            objectClickCounts[objectID] = 1;
        else
            objectClickCounts[objectID]++;

        if (requireSecondClick)
        {
            // Jika butuh klik ke-2
            if (objectClickCounts[objectID] == 2)
            {
                ShowTutorial();
            }
        }
        else
        {
            // Tampilkan hanya di klik pertama
            if (objectClickCounts[objectID] == 1)
            {
                ShowTutorial();
            }
        }
    }

    void ShowTutorial()
    {
        if (panelObject == null || tutorialAnimator == null) return;

        panelObject.SetActive(true);
        tutorialAnimator.Play(animationName, 0, 0f);
        tutorialActive = true;

        Invoke(nameof(HidePanel), displayDuration);
    }

    void HidePanel()
    {
        if (tutorialActive)
        {
            panelObject.SetActive(false);
            tutorialActive = false;
        }
    }

    private void OnDestroy()
    {
        if (tutorialActive && panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}
