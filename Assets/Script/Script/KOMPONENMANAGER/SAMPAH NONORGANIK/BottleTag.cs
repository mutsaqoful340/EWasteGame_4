using UnityEngine;
using System.Collections.Generic;

public class BotolTutorialTrigger : MonoBehaviour
{
    private static HashSet<string> clickedBottleIDs = new HashSet<string>();

    public string bottleID;
    public GameObject panelObject;
    public Animator tutorialAnimator;
    public string animationName = "BotolTutorial_SlideIn";
    public float displayDuration = 3f;

    private bool tutorialActive = false;

    void OnMouseDown()
    {
        if (!clickedBottleIDs.Contains(bottleID))
        {
            clickedBottleIDs.Add(bottleID);
            ShowTutorial();
        }
    }

    void ShowTutorial()
    {
        if (panelObject == null || tutorialAnimator == null) return;

        panelObject.SetActive(true);
        tutorialAnimator.Play(animationName, 0, 0f);
        tutorialActive = true;

        Invoke("HidePanel", displayDuration);
    }

    void HidePanel()
    {
        if (tutorialActive)
        {
            panelObject.SetActive(false);
            tutorialActive = false;
        }
    }

    // 👇 Panel langsung hilang jika botol dihancurkan
    private void OnDestroy()
    {
        if (tutorialActive && panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}
