using UnityEngine;
using System.Collections.Generic;

public class TutorialTrigger : MonoBehaviour
{
    // Simpan semua ID objek yang sudah pernah dipakai tutorial
    private static HashSet<string> shownObjectIDs = new HashSet<string>();

    [Header("Tutorial Settings")]
    public string objectID;                        // Unik per objek, misalnya "BotolA", "LaptopB"
    public GameObject panelObject;                 // Panel tutorial
    public Animator tutorialAnimator;              // Animator panel
    public string animationName = "SlideIn";       // Nama animasi slide-in
    public float displayDuration = 3f;             // Lama tampil panel

    private bool tutorialActive = false;

    void OnMouseDown()
    {
        if (!shownObjectIDs.Contains(objectID))
        {
            shownObjectIDs.Add(objectID);
            ShowTutorial();
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
