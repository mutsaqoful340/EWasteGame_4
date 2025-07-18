using UnityEngine;
using System.Collections.Generic;

public class TutorialTrigger : MonoBehaviour
{
    private static Dictionary<string, int> objectClickCounts = new Dictionary<string, int>();

    [Header("Tutorial Settings")]
    public string objectID;
    public GameObject panelObject;
    //public float displayDuration = 3f;

    //public Animator tutorialAnimator;
    //public string animationName = "SlideIn";

    [Header("Optional Behavior")]
    public bool requireSecondClick = false;

    [Header("Advanced Options")]
    public bool showOnlyOnce = true;   // ✅ Panel hanya tampil sekali

    private bool tutorialActive = false;
    private bool hasShown = false;     // ✅ Untuk tracking apakah sudah pernah tampil

    void OnMouseDown()
    {
        if (hasShown && showOnlyOnce) return;

        if (string.IsNullOrEmpty(objectID))
        {
            Debug.LogWarning("objectID belum diisi di: " + gameObject.name);
            return;
        }

        if (!objectClickCounts.ContainsKey(objectID))
            objectClickCounts[objectID] = 0;

        objectClickCounts[objectID]++;

        if (requireSecondClick)
        {
            if (objectClickCounts[objectID] == 2)
            {
                ShowTutorial();
            }
        }
        else
        {
            if (objectClickCounts[objectID] == 1)
            {
                ShowTutorial();
            }
        }
    }

    public void ShowTutorial()
    {
        if (hasShown && showOnlyOnce) return;

        if (panelObject == null) return;

        panelObject.SetActive(true);
        //tutorialAnimator.Play(animationName, 0, 0f);
        tutorialActive = true;
        hasShown = true; // ✅ tandai sudah tampil

        //Invoke(nameof(HidePanel), displayDuration);
    }

    //void HidePanel()
    //{
    //    if (tutorialActive)
    //    {
    //        panelObject.SetActive(false);
    //        tutorialActive = false;
    //    }
    //}

    private void OnDestroy()
    {
        if (tutorialActive && panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}
